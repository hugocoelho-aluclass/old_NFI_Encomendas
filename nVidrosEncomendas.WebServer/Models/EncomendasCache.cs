using iTextSharp.text;
using NVidrosEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Data.Entity;

namespace NVidrosEncomendas.WebServer.Models
{
    public class EncomendasCache
    {
        public static bool CacheDisabled = true;

        //public static bool UpdateCache = true;

        private static bool updateCache = true;
        private static bool updateCacheIncludes1 = true;


        public static bool UpdateCache
        {
            get { return false; }
            set
            {
                updateCache = true;
                updateCacheIncludes1 = true;
            }
        }


        private static IEnumerable<Models.Encomendas> _todasEncomendas { get; set; }
        private static IEnumerable<Models.Encomendas> _todasEncomendasIncludes1 { get; set; }

        private static bool updating = false;
        private static bool updatingIncludes1 = false;

        public static List<Models.Encomendas> GetEncomendasLista()
        {
            while (updating && !CacheDisabled)
            {
                Thread.Sleep(100);
            }
            if (updateCache == true || _todasEncomendas == null || CacheDisabled)
            {
                updating = true;
                EncomendasBL ebl = new EncomendasBL();
                _todasEncomendas = ebl.EncomendasLista();
                updating = false;
                UpdateCache = false;
            }
            return _todasEncomendas.ToList();

        }

        public static List<Models.Encomendas> GetEncomendasListaComClienteETipoEncomenda()
        {
            while (updatingIncludes1 && !CacheDisabled)
            {
                Thread.Sleep(100);
            }
            if (updateCacheIncludes1 == true || _todasEncomendasIncludes1 == null || CacheDisabled)
            {
                updatingIncludes1 = true;
                EncomendasBL ebl = new EncomendasBL();
                _todasEncomendasIncludes1 = ebl.DbContext.Encomendas
                     .Include(x => x.TiposEncomenda)
                     .Include(x => x.TiposEncomenda.Select(z => z.TipoEncomendas))
                     .Include(x => x.Cliente)
                     .Include(x => x.SerieDoc);

                updatingIncludes1 = false;
                updateCacheIncludes1 = false;
            }
            return _todasEncomendasIncludes1.ToList();

        }
    }
}