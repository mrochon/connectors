    internal class Script: ScriptBase
    {
        public override async Task<HttpResponseMessage> ExecuteAsync()
        {
            // Get token
            var tokenReq = new HttpRequestMessage(HttpMethod.Post, "https://tokenserver???")
            {
                Content = new StringContent("UserId=abc;Pwd=pwd", Encoding.UTF8, "applications/xyz???")
            };
            var response = await this.Context.SendAsync(tokenReq, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            
            // Add token to header and get data
            if(response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                this.Context.Request.Headers.Add("Cookie", token);
                return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }

            response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = CreateJsonContent("Token acquisition failed");
            return response;
        }
    }