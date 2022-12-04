using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraph
{
    interface IController
    {
        IModel Model { set;  get; }
        //IEventHandler EventHandler { get; }
        string str();
        string str2 { set; get; }
    }
    internal class Controller : IController
    {
        public IModel Model { set; get; }
        public string str2 { set; get; }
        //public IEventHandler EventHandler { get; }

        public Controller(IModel model)//, IEventHandler eventHandler)
        {
            this.Model = model;
            str2 = "safsadf";
            //this.EventHandler = eventHandler;
        }
        public string str()
        {
            return "AAAAA";
        }
    }
}
