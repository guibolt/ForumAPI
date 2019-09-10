using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Util
{
    public static class Arquivo
    {
        public static string arquivoDb = AppDomain.CurrentDomain.BaseDirectory + "db.json";
        public static Armazenamento Recuperar(Armazenamento Arquivo)
        {
            try
            {
                return JsonConvert.DeserializeObject<Armazenamento>(File.ReadAllText(arquivoDb)); 
            }
            catch (Exception ) { return default;}
        }
        public static void Salvar(Armazenamento Arquivo)
        {
            try
            {
                File.WriteAllText(arquivoDb, JsonConvert.SerializeObject(Arquivo));
            }
            catch (JsonException ex)

            {
                throw ex;
            }
        }
    }
}
