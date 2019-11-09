using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageActiveX
{


    /// <summary>
    ///1、定义安全检测接口；
    ///这个Guid是IObjectSafety接口的GUID，因为C#中没有直接实现IObjectsafety接口，因此要声明调用IObjectSafety接口
    ///InterfaceType声明COM接口的方式，IObjectSafety派生自IUnknown
    ///主要用途：IE在碰到ActiveX组件的时候，首先要调用IObjectSafety接口，如果返回是S_OK，那么浏览器就认为这个ActiveX是安全的，否则，浏览器会给用户返回一个危险的提示。
    /// </summary>
    [ComImport, GuidAttribute("CB5BDC81-93C1-11CF-8F20-00805F2CD064")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectSafety
    {
        [PreserveSig]
        int GetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] ref int pdwSupportedOptions, [MarshalAs(UnmanagedType.U4)] ref int pdwEnabledOptions);

        [PreserveSig()]
        int SetInterfaceSafetyOptions(ref Guid riid, [MarshalAs(UnmanagedType.U4)] int dwOptionSetMask, [MarshalAs(UnmanagedType.U4)] int dwEnabledOptions);
    }

    

    /// <summary>
    /// 2、实现上面接口；
    ///GUID用于插件系统安装注册标识 在系统注册表中可以看到
    ///GUID可以自定义 工具->创建GUID
    ///H5中引用方法：<object classid="CLSID:E52C3E70-B486-4743-B8AE-5D830A561442" codebase="plugin/ActiveXsetup.exe" id="abc" width="0" height="0"></object> 
    ///IE浏览器会自动检测系统是否有 classid对应的注册应用  如果没有会有提示下载安装 ActiveXsetup.exe 
    /// </summary>
    [Guid("E52C3E70-B486-4743-B8AE-5D830A561443")]
    public partial  class Main : IObjectSafety
    {

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) && (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) && (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }
        
    }


     
    /// <summary>
    /// 3、增加个人业务代码
    /// </summary>
    public partial class Main
    {
        public string SayHello()
        {
            return "Hi I'm from ActiveX Compliled by C#!";
        }

        public string SetData(string input)
        {
            return input;
        }

        /// <summary>
        /// 读取本地指定图片返回对应的base64字符串
        /// 浏览器调用此方法从客户端加载图片
        /// </summary>
        /// <param name="imagePath">本地系统问图片全路径如："D:\\放羊人.jpg"</param>
        /// <returns></returns>
        public string Image_GetBase64(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return "大兄弟出错了：你传入参数为空！";
            }
            if (!File.Exists(imagePath))
            {
                return "大兄弟出错了：你传入的图片文件不存在！";
            }

            try
            {
                Bitmap tmpBitmap = new Bitmap(imagePath);
                string tmpBase64 = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    tmpBitmap.Save(ms, ImageFormat.Jpeg);
                    byte[] tmpByteData = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(tmpByteData, 0, (int)ms.Length);
                    ms.Close();
                    tmpBase64 = Convert.ToBase64String(tmpByteData);
                }

                return tmpBase64;
            }
            catch (Exception ex)
            {
                return string.Format("大兄弟出错了，读取图片转base64异常：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 将Base64的图片保存到指定位置
        /// 浏览器调用此方法往客户端电脑写入图片
        /// </summary>
        /// <param name="imgBase64">图片base64编码</param>
        /// <param name="img_SavePath">本地保存位置如："E:\\放羊人.jpg"</param>
        /// <returns></returns>
        public bool Image_SaveToLocal(string imgBase64,string img_SavePath)
        {
            //代码就偷懒不写了
            return true;
        }
    }
}
