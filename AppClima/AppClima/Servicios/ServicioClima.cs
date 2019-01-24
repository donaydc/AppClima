using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppClima.Clases;


namespace AppClima.Servicios
{
    public static class ServicioClima
    {
        static string Key = "3ba4f70ad5929bf6b91f8ed6c0dd4fd7";

        public static async Task<Clima> ConsultarClima(string Ciudad)
        {
            var conexion = $"http://api.openweathermap.org/data/2.5/weather?q={Ciudad}&appid={Key}";

            using (var cliente = new HttpClient())
            {
                var peticion = await cliente.GetAsync(conexion);
                if (peticion != null)
                {
                    var json = peticion.Content.ReadAsStringAsync().Result;
                    var datos = (JContainer)JsonConvert.DeserializeObject(json);
                    if (datos["weather"] != null)
                    {
                        var clima = new Clima();
                        clima.Titulo = (string)datos["name"];
                        clima.Temperatura = ((float)datos["main"]["temp"] - 273.15).ToString("N2") + " °C";
                        clima.Viento = (string)datos["wind"]["speed"] + " mph";
                        clima.Humedad = (string)datos["main"]["humidity"] + " %";
                        clima.Visibilidad = (string)datos["weather"][0]["main"];

                        var fechaBase = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        var amanecer = fechaBase.AddSeconds((double)datos["sys"]["sunrise"]);
                        var ocaso = fechaBase.AddSeconds((double)datos["sys"]["sunset"]);
                        amanecer = amanecer.AddHours(-5);
                        ocaso = ocaso.AddHours(-17);
                        clima.Amanecer = amanecer.Hour.ToString() + ":" + amanecer.Minute.ToString() + " AM";
                        clima.Ocaso = ocaso.Hour.ToString() + ":" + ocaso.Minute.ToString() + " PM";
                        return clima;
                    }
                }
            }
            return default(Clima);
        }
    }
}
