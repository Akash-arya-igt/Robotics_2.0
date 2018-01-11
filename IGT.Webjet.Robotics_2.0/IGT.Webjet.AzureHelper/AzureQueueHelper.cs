using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace IGT.Webjet.AzureHelper
{
    public class AzureQueueHelper
    {
        private CloudQueue _messageQueue;

        public AzureQueueHelper(string _pQName, string _pConnstionstring)
        {
            var storageAccount = CloudStorageAccount.Parse(_pConnstionstring);

            // Create the CloudQueueClient object for the storage account.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Get a reference to the CloudQueue named "messageQueue"
            _messageQueue = queueClient.GetQueueReference(_pQName);

            Task createQTask = _messageQueue.CreateIfNotExistsAsync();
            createQTask.Wait();
        }

        public async Task AddMsgAsync(string _pMsg)
        {
            CloudQueueMessage message = new CloudQueueMessage(_pMsg);
            await _messageQueue.AddMessageAsync(message);
        }

        public void AddMsg(string _pMsg)
        {
            Task tskAddMsg = AddMsgAsync(_pMsg);
            tskAddMsg.Wait();
        }

        public async Task<string> ReadMsgAsync()
        {
            CloudQueueMessage peekedMessage = await _messageQueue.PeekMessageAsync();
            return peekedMessage.AsString;
        }

        public string ReadMsg()
        {
            return ReadMsgAsync().Result;
        }

        public async Task<string> GetMsgAsync()
        {
            string strMsg = string.Empty;
            CloudQueueMessage retrievedMessage = await _messageQueue.GetMessageAsync();

            if (retrievedMessage != null)
            {
                strMsg = retrievedMessage.AsString;
                await _messageQueue.DeleteMessageAsync(retrievedMessage);
            }

            return strMsg;
        }

        public string GetMsg()
        {
            return GetMsgAsync().Result;
        }

        public async Task<int> GetQCountAsync()
        {
            await _messageQueue.FetchAttributesAsync();
            return _messageQueue.ApproximateMessageCount == null ? 0 : _messageQueue.ApproximateMessageCount.Value;
        }

        public int GetQCount()
        {
            return GetQCountAsync().Result;
        }

        public async Task<List<string>> GetMsgsAsync(int _pMsgCount, int _pMsgVisibilityTimeout)
        {
            List<string> lstStringMsg = new List<string>();
            var lstMsg = await _messageQueue.GetMessagesAsync(_pMsgCount, TimeSpan.FromMinutes(_pMsgVisibilityTimeout), null, null);
            foreach (var msg in lstMsg)
            {
                lstStringMsg.Add(msg.AsString);
                await _messageQueue.DeleteMessageAsync(msg);
            }

            return lstStringMsg;
        }

        public List<string> GetMsgs(int _pMsgCount, int _pMsgVisibilityTimeout)
        {
            return GetMsgsAsync(_pMsgCount, _pMsgVisibilityTimeout).Result;
        }
    }
}
