namespace DaGetV2.Gui.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Shared.ApiTool;

    public abstract class ModelBase
    {
        public ModelBase()
        {
            Errors = new List<string>();
        }

        public IList<string> Errors { get; set; }

        public string Details { get; set; }

        public bool HasErrors
        {
            get
            {
                return Errors != null && Errors.Count > 0;
            }
        }

        public async Task<bool> ValidateAsync(HttpResponseMessage response)
        {
            if (response != null && (int)response.StatusCode >= 300)
            {
                try
                {
                    var errorApiResult = JsonConvert.DeserializeObject<ApiErrorResultDto>(await response.Content.ReadAsStringAsync());
                    if (errorApiResult != null)
                    {
                        Errors = errorApiResult.Message.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                        Details = errorApiResult.Details;
                    }
                }
                catch
                {
                    Errors.Add($"{(int)response.StatusCode} {response.ReasonPhrase}");
                    return false;
                }
            }
            return !HasErrors;
        }
    }
}
