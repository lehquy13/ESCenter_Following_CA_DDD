// using Microsoft.AspNetCore.Http;
//
// namespace ESCenter.Admin.Host.Middlewares;
//
// public class AuthorizeMiddleware : IMiddleware
// {
//     public Task InvokeAsync(HttpContext context, RequestDelegate next)
//     {
//         var token = context.Request.Cookies["AccessToken"];
//
//         if (!string.IsNullOrEmpty(token) &&
//             !context.Request.Headers.ContainsKey("Authorization"))
//         {
//             context.Request.Headers.Append("Authorization", "Bearer " + token);
//         }
//
//         await next();
//     }
// }
//
// // options.Events = new JwtBearerEvents()
// // {
// //     OnMessageReceived = context =>
// //     {
// //         try
// //         {
// //             var token = context.HttpContext.Session.GetString("access_token");
// //             if (token != null)
// //             {
// //                 context.Token = token;
// //             }
// //         }
// //         catch (Exception e)
// //         {
// //             Console.WriteLine(e);
// //         }
// //
// //         return Task.CompletedTask;
// //     },
// // };