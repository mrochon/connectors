public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        // Get token
        //var tokenReq = new HttpRequestMessage(HttpMethod.Post, "https://connectorwebutilities.azurewebsites.net/api/HandleRequest")
        var tokenReq = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/c3645e09-d602-4e25-950c-5850e383d6f2/oauth2/v2.0/token")
        {
            Content = new StringContent("client_id=07c916f0-5993-48dc-8a14-36a5102d5677&client_secret=ufT8Q~OkHvB~EE2pFeTxVvQ_Rsm7Xw1fRlRvddmV&scope=https%3A%2F%2Fgraph.microsoft.com%2F.default&grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
        };
        var response = await this.Context.SendAsync(tokenReq, CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        
        // Add token to header and get data
        if(response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            this.Context.Request.Headers.Add("Authorization", $"Bearer {token}");
            return await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent("Token acquisition failed");
        return response;
    }
}