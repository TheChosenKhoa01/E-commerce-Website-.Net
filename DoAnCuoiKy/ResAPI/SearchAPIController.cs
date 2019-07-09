using DoAnCuoiKy.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;

namespace DoAnCuoiKy.ResAPI
{
    [RoutePrefix("api/product")]
    public class SearchAPIController : ApiController
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //[HttpGet]
        //[Route("find")]
        //public HttpResponseMessage find()
        //{
        //    try
        //    {
        //        //var productNames = 
        //        //var response = new HttpResponseMessage(HttpStatusCode.OK);
        //        //response.Content = new StringContent(JsonConvert.SerializeObject(Products));
        //        //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        //return response;
        //    }
        //    catch
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);
        //    }
        //}
        [HttpGet]
        [Route("laysanpham")]
        public List<SanPham> LaySanPham()
        {
            return db.SanPhams.ToList();

        }

    }
}
