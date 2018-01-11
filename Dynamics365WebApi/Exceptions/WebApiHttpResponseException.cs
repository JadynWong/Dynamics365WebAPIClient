using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dynamics365WebApi.Exceptions
{
    /// <summary>
    /// 解析WebAPI返回的异常信息
    /// </summary>
    public class WebApiHttpResponseException : System.Exception
    {
        #region Properties

        private static string _stackTrace;

        /// <summary>
        /// StackTrace – 引发了异常（如果有）时 Dynamics 365 服务器的调用堆栈中直接框架的字符串表示。
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public override string StackTrace
        {
            get { return _stackTrace; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// 构造函数利用此类的实例，并且需要一个 HttpContent 参数和一个可选的内部异常参数。
        /// Initializes a new instance of the CrmHttpResponseException class.
        /// </summary>
        /// <param name="content">The populated HTTP content in Json format.</param>
        public WebApiHttpResponseException(HttpContent content)
            : base(ExtractMessageFromContent(content))
        {
        }

        /// <summary>
        /// 构造函数利用此类的实例，并且需要一个 HttpContent 参数和一个可选的内部异常参数。
        /// Initializes a new instance of the CrmHttpResponseException class.
        /// </summary>
        /// <param name="content">The populated HTTP content in Json format.</param>
        /// <param name="innerexception">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public WebApiHttpResponseException(HttpContent content, Exception innerexception)
            : base(ExtractMessageFromContent(content), innerexception)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// ExtractMessageFromContent – 此静态方法从指定的 HTTP 内容参数提取错误消息。
        /// Extracts the CRM specific error message and stack trace from an HTTP content. 
        /// </summary>
        /// <param name="content">The HTTP content in Json format.</param>
        /// <returns>The error message.</returns>
        private static string ExtractMessageFromContent(HttpContent content)
        {
            string message = String.Empty;
            string downloadedContent = content.ReadAsStringAsync().Result;
            if (content.Headers.ContentType.MediaType.Equals("text/plain"))
            {
                message = downloadedContent;
            }
            else if (content.Headers.ContentType.MediaType.Equals("application/json"))
            {
                JObject jcontent = (JObject) JsonConvert.DeserializeObject(downloadedContent);
                IDictionary<string, JToken> d = jcontent;

                // An error message is returned in the content under the 'error' key. 
                if (d.ContainsKey("error"))
                {
                    JObject error = (JObject) jcontent.Property("error").Value;
                    message = (String) error.Property("message").Value;
                }
                else if (d.ContainsKey("Message"))
                    message = (String) jcontent.Property("Message").Value;

                if (d.ContainsKey("StackTrace"))
                    _stackTrace = (String) jcontent.Property("StackTrace").Value;
            }
            else if (content.Headers.ContentType.MediaType.Equals("text/html"))
            {
                message = "HTML content that was returned is shown below.";
                message += "\n\n" + downloadedContent;
            }
            else
            {
                message = String.Format("No handler is available for content in the {0} format.",
                    content.Headers.ContentType.MediaType.ToString());
            }

            return message;

            #endregion Methods
        }
    }
}