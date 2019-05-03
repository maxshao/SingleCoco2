using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SingleCoco.Infrastructure.Controllers;
using System;
using System.Collections.Generic;

namespace SingleCoco
{

    public class ControllersBase : Controller
    {
        public JsonResult Json<T>(string d, bool b) where T : class
        {
            Message<T> msg = new Message<T>(b, d);
            return base.Json(msg);
        }

        public JsonResult Json<T>(T data) where T : class
        {
            Message<T> msg = new Message<T>(true, data);
            return base.Json(msg);
        }

        public JsonResult Json<T>(T t, object d) where T : class
        {
            Message<T> msg = new Message<T>(true, t, d);
            return base.Json(msg);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public JsonResult Json<T>(Tuple<int, int, int, IEnumerable<T>> d) where T : class
        {
            Message<T> msg = new Message<T>(true, d.Item4);
            //msg.PageIndex = d.Item1;
            //msg.PageSize = d.Item2;
            //msg.PageCount = d.Item3;
            return base.Json(msg);
        }
    }

    public class Message<T> where T : class
    {
        public Message(bool status)
        {
            Status = status;
            List = null;
            Data = null;
        }

        public Message(bool status, string msg)
        {
            Status = status;
            Msg = msg;
        }
        public Message(bool status, T value)
        {
            Status = status;
            List = value;
            Data = null;
        }

        public Message(bool status, T value, object d)
        {
            Status = status;
            List = value;
            Data = d;
        }

        public Message(bool status, IEnumerable<T> value)
        {
            Status = status;
            List = value;
            Data = null;
        }

        /// <summary>
        /// 0:false,1:success
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 列表
        /// </summary>
        public object List { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public object Data { get; set; }

        public string Msg { get; set; }
    }

}




