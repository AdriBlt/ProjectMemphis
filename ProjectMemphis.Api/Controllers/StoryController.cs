using System.Web.Http;
using System.Web.Mvc;
using ProjectMemphis.Data;

namespace ProjectMemphis.Api.Controllers
{
    [System.Web.Mvc.RoutePrefix("rest/story")]
    public class StoryController : Controller
    {
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("listIds")]
        public ActionResult ListIds() => Content("ok");


        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Route("add")]
        public ActionResult Add([FromBody] Story story) => Content("ok");

        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Route("{id?}")]
        public ActionResult Get(string id) => Content("ok" + id);

        [System.Web.Mvc.HttpPut]
        [System.Web.Mvc.Route("{id?}")]
        public ActionResult Update(string id) => Content("ok");

        [System.Web.Mvc.HttpDelete]
        [System.Web.Mvc.Route("{id?}")]
        public ActionResult Delete(string id) => Content("ok");
    }
}