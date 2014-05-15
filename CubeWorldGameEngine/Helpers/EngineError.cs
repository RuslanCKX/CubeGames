using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CubeWorldGameEngine.Helpers
{

    public class EngineErrorEventArgs : EventArgs
    {
        public EngineErrorEnum EngineErrorID { get; set; }
        public String EngineErrorStringMsg;

        public EngineErrorEventArgs(EngineErrorEnum id, string msg)
        {
            EngineErrorID = id;
            EngineErrorStringMsg = msg;
        }

        public EngineErrorEventArgs() { }

    }

    public delegate void EngineErrorEventHandler(object sender, EngineErrorEventArgs e);

    public class EngineErrorControl
    {
        public event EngineErrorEventHandler EngineError;
        
        public EngineErrorControl()
        {
        }

        public void RaiseEngineErrorEvent(EngineErrorEnum id, string msg = "")
        {
            EngineErrorEventArgs e = new EngineErrorEventArgs {EngineErrorID = id};

            switch (id)
            {
                case EngineErrorEnum.ResoursesPacksFileNotFound:
                    e.EngineErrorStringMsg =
                        "Не найден файл с описанием ресурсов. Проверьте наличие файла " + EngineMain.AppPath +
                        EngineMain.ResPath + EngineMain.ResPacksFileName;
                    break;
                case EngineErrorEnum.ResoursesPacksFileXMLReadError:
                    e.EngineErrorStringMsg =
                        "Ошибка чтения структуры XML файла, содержащего информацию о пакеах игры. Если вы изменяли его самостоятельно проверьте правильность. Иначе переустановите игру или обратитесь в службу поддержки. Файл: " + EngineMain.AppPath + EngineMain.ResPath + EngineMain.ResPacksFileName;
                    break;
                case EngineErrorEnum.ResoursesPacksFileXMLNodeNotFound:
                    e.EngineErrorStringMsg =
                        "Не найден узел " + msg + " в файле " + EngineMain.AppPath + EngineMain.ResPath +
                        EngineMain.ResPacksFileName; 
                    break;
                case EngineErrorEnum.ResoursesPacksFileXMLNodeAttributesNotFound:
                    e.EngineErrorStringMsg =
                        "Не найдены параметр " + msg + " в файле " + EngineMain.AppPath + EngineMain.ResPath +
                        EngineMain.ResPacksFileName;
                    break;
                case EngineErrorEnum.ResoursesPacksFileXMLNodeInvalidValue:
                    e.EngineErrorStringMsg =
                        "Не верное значение " + msg  + " в файле " + EngineMain.AppPath + EngineMain.ResPath +
                        EngineMain.ResPacksFileName;
                    break;
                case EngineErrorEnum.ResoursesPackInfoFileNotFound:
                    e.EngineErrorStringMsg =
                        "Файл описания ресурс пакета " + msg + " в файле " + EngineMain.AppPath + EngineMain.ResPath +
                        EngineMain.ResPacksFileName;
                    break;

            }

            On_engineError(e);
        }

        protected virtual void On_engineError(EngineErrorEventArgs e)
        {
            if (EngineError != null)
            {
                EngineError(this, e);
            }
        }
    }

    public enum EngineErrorEnum
    {
        ResoursesPacksFileNotFound,
        ResoursesPacksFileXMLReadError,
        ResoursesPacksFileXMLNodeNotFound,
        ResoursesPacksFileXMLNodeAttributesNotFound,
        ResoursesPacksFileXMLNodeInvalidValue,
        ResoursesPackInfoFileNotFound
    }

   

}
