using Newtonsoft.Json.Linq;
using NanoRpc.Exceptions;
using NanoRpc.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NanoRpc
{
    // TODO: COMPLETE THE TODOS
    public class RpcClient
    {
        protected readonly HttpClient httpClient = new HttpClient();
        
        public Uri Uri { get; private set; }

        public RpcClient(Uri uri)
        {
            this.Uri = uri;
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "NanoRpc.NET");
        }

        private async Task<bool> ResponseHasError(string response)
        {
            return await Task.Run(() => response.Contains("\"error\""));
        }

        private async Task ThrowErrorException(string response)
        {
            // TODO: Parse errors
        }

        private async Task PostMessage(string message)
        {
            using (StringContent content = new StringContent(message))
            {
                using (HttpResponseMessage response = await httpClient.PostAsync(this.Uri, content))
                {
                    string rawJson = await response.Content.ReadAsStringAsync();

                    if (await ResponseHasError(rawJson))
                    {
                        try
                        {
                            await ThrowErrorException(rawJson);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                }
            }
        }

        private async Task<T> PostMessage<T>(string message = "") where T : class
        {
            using (StringContent content = new StringContent(message))
            {
                using (HttpResponseMessage response = await httpClient.PostAsync(this.Uri, content))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));

                    string rawJson = await response.Content.ReadAsStringAsync();

                    if (await ResponseHasError(rawJson))
                    {
                        try
                        {
                            await ThrowErrorException(rawJson);
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }

                    if (typeof(T) == typeof(string))
                    {
                        return rawJson as T;
                    }
                    else
                    {
                        using (MemoryStream memStream = new MemoryStream(Encoding.Default.GetBytes(rawJson)))
                        {
                            return jsonSerializer.ReadObject(memStream) as T;
                        }
                    }
                }
            }
        }

        #region Helper Classes
        // TODO: Find a better way to do this while still using the 'PostData' function
        private class json_account { public string account { get; set; } }
        private class json_account_list { public List<string> accounts { get; set; } }
        private class json_balances { public List<AccountBalance> balances { get; set; } }
        private class json_block { public string block { get; set; } }
        private class json_block_count { public UInt64 block_count { get; set; } }
        private class json_block_list { public List<string> blocks { get; set; } }
        private class json_contents { public string contents { get; set; } }
        private class json_count { public UInt64 count { get; set; } }
        private class json_hash { public string hash { get; set; } }
        private class json_history { public List<Transaction> history { get; set; } }
        private class json_key { public string key { get; set; } }
        private class json_moved { public bool moved { get; set; } }
        private class json_removed { public bool removed { get; set; } }
        private class json_representative { public string representative { get; set; } }
        private class json_valid { public int valid { get; set; } }
        private class json_weight { public BigInteger weight { get; set; } }
        #endregion

        #region Account Commands
        /// <summary>
        /// Returns how many RAW is owned and how many have not yet been received by account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <returns><see cref="AccountBalance"/></returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<AccountBalance> GetAccountBalanceAsync(string account)
        {
            AccountBalance result = await PostMessage<AccountBalance>("{\"action\": \"account_balance\", \"account\": \"" + account + "\"}");
            result.Account = account;

            return result;
        }

        /// <summary>
        /// Get number of blocks for a specific account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <returns>Block Count</returns>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundException"/>
        public async Task<UInt64> GetAccountBlockCountAsync(string account)
        {
            json_block_count result = await PostMessage<json_block_count>("{\"action\": \"account_block_count\", \"account\": \"" + account + "\"}");

            return result.block_count;
        }

        /// <summary>
        /// Returns frontier, open block, change representative block, balance, last modified timestamp from local database & block count for account
        /// Optionally returns representative, voting weight, pending balance for account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <param name="transaction">Tell node to return the representative</param>
        /// <param name="weight">Tell node to return voting weight</param>
        /// <param name="pending">Tell node to return pending balance amount</param>
        /// <returns><see cref="AccountInformation"/></returns>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundException"/>
        public async Task<AccountInformation> GetAccountInformationAsync(string account, bool representative = false, bool weight = false, bool pending = false)
        {
            AccountInformation result = await PostMessage<AccountInformation>("{ \"action\": \"account_info\", " + 
                "\"account\": \"" + account + "\", \"representative\": \"" + representative + "\", \"weight\": \"" + weight + "\", \"pending\": \"" + pending + "\" }");
            result.Account = account;

            return result;
        }
        
        /// <summary>
        /// Creates a new account, insert next deterministic key in wallet
        /// </summary>
        /// <param name="wallet">Nano wallet</param>
        /// <param name="work">Toggle work generation after creating account</param>
        /// <returns>Accoutn Number</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="WalletLockedException"/>
        public async Task<string> CreateAccountAsync(string wallet, bool work = true)
        {
            json_account result = await PostMessage<json_account>("{ \"action\": \"account_create\", " + 
                "\"wallet\": \"" + wallet + "\", \"work\": \"" + work + "\" }");

            return result?.account;
        }
        
        /// <summary>
        /// Get account number for the public key
        /// </summary>
        /// <param name="key">Public Key</param>
        /// <returns>Account Number</returns>
        /// <exception cref="BadPublicKeyException"/>
        public async Task<string> GetAccountAsync(string key)
        {
            json_account result = await PostMessage<json_account>("{ \"action\": \"account_get\", \"key\": \"" + key + "\" }");

            return result?.account;
        }

        /// <summary>
        /// Reports send/receive information for a account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <param name="count">Number of historical transactions to return</param>
        /// <returns>Send and receive transactions for account <see cref="Transaction"/></returns>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public async Task<Transaction[]> GetAccountHistoryAsync(string account, UInt64 count)
        {
            json_history result = await PostMessage<json_history>("{ \"action\": \"account_history\", " + 
                "\"account\": \"" + account + "\", \"count\": \"" + count + "\" }");

            return result?.history?.ToArray();
        }

        /// <summary>
        /// Lists all the accounts inside wallet
        /// </summary>
        /// <param name="wallet">Nano wallet</param>
        /// <returns>Accounts inside the wallet</returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public async Task<string[]> GetAccountListAsync(string wallet)
        {
            json_account_list result = await PostMessage<json_account_list>("{ \"action\": \"account_list\", \"wallet\": \"" + wallet + "\" }");

            return result?.accounts?.ToArray();
        }

        // TODO: No "Wallet is locked" in RPC code?
        /// <summary>
        /// Moves accounts from source to wallet
        /// </summary>
        /// <param name="wallet">Nano destination wallet number</param>
        /// <param name="source">Nano source wallet number</param>
        /// <param name="accounts">Nano accounts to move</param>
        /// <returns>Always true, an exception being thrown means false</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadSourceNumberException"/>
        /// <exception cref="SourceNotFoundException"/>
        public async Task<bool> MoveAccountsAsync(string wallet, string source, string[] accounts)
        {
            json_moved result = await PostMessage<json_moved>("{ \"action\": \"account_move\", " +
                "\"wallet\": \"" + wallet + "\", " +
                "\"source\": \"" + source + "\", " +
                "\"accounts\" : [ " + accounts.ToJsonString(false) + " ] }");

            return result.moved;
        }

        /// <summary>
        /// Get the public key for account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <returns>Nano account's public key</returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<string> GetAccountKeyAsync(string account)
        {
            json_key result = await PostMessage<json_key>("{ \"action\": \"account_key\", " + 
                "\"account\": \"" + account + "\" }");

            return result?.key;
        }

        /// <summary>
        /// Remove account from wallet
        /// </summary>
        /// <param name="wallet">Nano wallet number</param>
        /// <param name="account">Nano account number</param>
        /// <returns>Always true, if something went wrong an exception is thrown</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="WalletLockedException"/>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundInWalletException"/>
        public async Task<bool> RemoveAccountAsync(string wallet, string account)
        {
            json_removed result = await PostMessage<json_removed>("{ \"action\": \"account_remove\", " + 
                "\"wallet\": \"" + wallet + "\", \"account\": \"" + account +  "\" }");

            return result.removed;
        }
        
        /// <summary>
        /// Returns the representative for account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <returns>Account representative</returns>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundException"/>
        public async Task<string> GetAccountRepresentativeAsync(string account)
        {
            json_representative result = await PostMessage<json_representative>("{ \"action\": \"account_representative\", " + 
                "\"account\": \"" + account + "\" }");

            return result?.representative;
        }
        
        /// <summary>
        /// Sets the representative for account in wallet
        /// </summary>
        /// <param name="wallet">Nano wallet for the account</param>
        /// <param name="account">Nano account number</param>
        /// <param name="representative">New Nano representative</param>
        /// <param name="work">Precomputed work</param>
        /// <returns>Block hash</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundException"/>
        /// <exception cref="InvalidWorkException"/>
        /// <exception cref="BadWorkException"/>
        public async Task<string> SetAccountRepresentativeAsync(string wallet, string account, string representative, string work = null)
        {
            string message;

            if (String.IsNullOrEmpty(work))
            {
                message = "{ \"action\": \"account_representative_set\", " +
                    "\"wallet\": \"" + wallet + "\", \"account\": \"" + account + "\", " +
                    "\"representative\": \"" + representative + "\" }";
            }
            else
            {
                message = "{ \"action\": \"account_representative_set\", " +
                    "\"wallet\": \"" + wallet + "\", \"account\": \"" + account + "\", " +
                    "\"representative\": \"" + representative + "\", \"work\": \"" + work + "\" }";
            }

            json_block result = await PostMessage<json_block>(message);

            return result?.block;
        }

        /// <summary>
        /// Returns the voting weight for account
        /// </summary>
        /// <param name="account">Nano account number</param>
        /// <returns>Account voting weight</returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<BigInteger> GetAccountWeightAsync(string account)
        {
            json_weight result = await PostMessage<json_weight>("{ \"action\": \"account_weight\", " + 
                "\"account\": \"" + account + "\" }");

            return result.weight;
        }

        /// <summary>
        /// Returns how many RAW is owned and how many have not yet been received by accounts list
        /// </summary>
        /// <param name="accounts">Nano account numbers</param>
        /// <returns>Balanaces for each Nano account</returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<AccountBalance[]> GetAccountsBalancesAsync(string[] accounts)
        {
            List<AccountBalance> result = new List<AccountBalance>();

            JObject resultJson = JObject.Parse(await PostMessage<string>("{ \"action\": \"accounts_balances\", " +
                "\"accounts\": [ " + accounts.ToJsonString(false) + " ] }"));

            // Couldn't find a way to parse this message using the built in .NET serialization so we do it manually
            // This gets the property 'balances' and attempts to convert it to a JObject named 'jBalances'
            if (resultJson["balances"] is JObject jBalances)
            {
                // If we were able to find and cast the balances to 'JObject' then we'll access all the json properties in it
                foreach (JProperty property in jBalances.Properties())
                {
                    // The name of the property is the account number (which is why we can't use the default lib, this is non-standard json)
                    string account = property.Name;

                    // Try to cast the value to a 'JObject' named 'jBalance'
                    if (property.Value is JObject jBalance)
                    {
                        // Try to convert the balance property to a JValue, if that works get the value or return null, 
                        // if it's not null try to convert to string and if that fails return null
                        BigInteger.TryParse((jBalance["balance"] as JValue)?.Value as String, out BigInteger balance);
                        BigInteger.TryParse((jBalance["pending"] as JValue)?.Value as String, out BigInteger pending);

                        result.Add(new AccountBalance()
                        {
                            Account = account,
                            Balance = balance,
                            Pending = pending
                        });
                    }
                    else
                    {
                        result.Add(new AccountBalance()
                        {
                            Account = account
                        });
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Creates new accounts, insert next deterministic keys in wallet up to count
        /// </summary>
        /// <param name="wallet">Nano wallet to store the accounts in</param>
        /// <param name="count">Number of accounts to create</param>
        /// <param name="work">Toggle work generation after account creation</param>
        /// <returns>List of new Nano accounts</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="InvalidCountLimitException"/>
        /// <exception cref="WalletNotFoundException"/>
        public async Task<string[]> CreateAccountsAsync(string wallet, UInt64 count, bool work = true)
        {
            json_account_list result = await PostMessage<json_account_list>("{ \"action\": \"accounts_create\", " + 
                "\"wallet\": \"" + wallet + "\", \"count\": \"" + count + "\", \"work\": \"" + work + "\" }");

            return result?.accounts?.ToArray();
        }

        /// <summary>
        /// Returns a list of pairs of account and block hash representing the head block for accounts list
        /// </summary>
        /// <param name="accounts">List of Nano accounts to get the frontiers for</param>
        /// <returns><see cref="AccountFrontier"/></returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<AccountFrontier[]> GetAccountsFrontiersAsync(string[] accounts)
        {
            List<AccountFrontier> result = new List<AccountFrontier>();

            JObject resultJson = JObject.Parse(await PostMessage<string>("{ \"action\": \"accounts_frontiers\", " +
                "\"accounts\": [ " + accounts.ToJsonString(false) + " ] }"));

            // Couldn't find a way to parse this message using the built in .NET serialization so we do it manually
            // This gets the property 'frontiers' and attempts to convert it to a JObject named 'jFrontiers'
            if (resultJson["frontiers"] is JObject jFrontiers)
            {
                // If we were able to find and cast the frontiers to 'JObject' then we'll access all the json properties in it
                foreach (JProperty property in jFrontiers.Properties())
                {
                    // The name of the property is the account number (which is why we can't use the default lib, this is non-standard json)
                    string account = property.Name;

                    // Try to cast the value to a 'JObject' named 'jFrontier'
                    if (property.Value is JValue jFrontier)
                    {
                        result.Add(new AccountFrontier()
                        {
                            Account = account,
                            BlockHash = jFrontier.Value as String
                        });
                    }
                    else
                    {
                        result.Add(new AccountFrontier()
                        {
                            Account = account
                        });
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Returns a list of block hashes which have not yet been received by these accounts
        /// Optional "threshold":
        /// Returns a list of pending block hashes with amount more or equal to threshold
        /// Optional "source":
        /// Returns a list of pending block hashes with amount and source accounts
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="count"></param>
        /// <param name="threshold"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<AccountPendingTransactions[]> GetAccountsPendingAsync(string[] accounts, UInt64 count, BigInteger threshold = default(BigInteger), bool source = false)
        {
            List<AccountPendingTransactions> result = new List<AccountPendingTransactions>();

            // Construct the message
            string message = "{ \"action\": \"accounts_pending\", " + 
                "\"accounts\": [ " + accounts.ToJsonString() + " ], \"count\": \"" + count + "\"";

            if (threshold != default(BigInteger))
            {
                message += ", \"threshold\": \"" + threshold + "\"";
            }

            if (source)
            {
                message += ", \"source\": \"" + source + "\"";
            }

            message += "}";

            JObject resultJson = JObject.Parse(await PostMessage<string>(message));

            // Due to the way data is sent back we have to do in manually in three different ways for this call too.
            if (resultJson["blocks"] is JObject jBlocks)
            {
                // Look through the accounts, luckily this is the same for all three results
                foreach (JProperty jAccount in jBlocks.Properties())
                {
                    string account = jAccount.Name;
                    List<PendingTransaction> pendingList = new List<PendingTransaction>();

                    // If they only supplied accounts and count then parse only the block hashes as an array
                    if (jAccount.Value is JArray jBlockArray)
                    {
                        foreach (JToken jBlock in jBlockArray)
                        {
                            pendingList.Add(new PendingTransaction()
                            {
                                Hash = jBlock.ToString()
                            });
                        }
                    }
                    // If they also supplied 'threshold' or 'source' then parse as an object
                    else if (jAccount.Value is JObject jBlockObject)
                    {
                        // Each property name is the pending account. This is stupid and makes parsing more difficult.
                        foreach (JProperty jBlock in jBlockObject.Properties())
                        {
                            string blockHash = jBlock.Name;

                            // If they supplied 'source' then we parse as an object and get the 'amount' and 'source' properties
                            if (jBlock.Value is JObject jPending)
                            {
                                BigInteger.TryParse((jPending["amount"] as JValue)?.Value as String, out BigInteger amount);

                                pendingList.Add(new PendingTransaction()
                                {
                                    Hash = jBlock.ToString(),
                                    Source = jPending["source"].ToString(),
                                    Amount = amount
                                });
                            }
                            // If they only supplied 'threshold' then we treat the value as a string
                            else
                            {
                                BigInteger.TryParse((jBlock.Value as JValue)?.Value as String, out BigInteger amount);

                                pendingList.Add(new PendingTransaction()
                                {
                                    Hash = jBlock.ToString(),
                                    Amount = amount
                                });
                            }
                        }
                    }
                    else
                    {
                        // TODO: Handle this?
                    }

                    // Add to the result
                    result.Add(new AccountPendingTransactions()
                    {
                        Account = account,
                        PendingTransactions = pendingList.ToArray()
                    });
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Check whether account is a valid account number
        /// </summary>
        /// <param name="account">Nano accoutn number</param>
        /// <returns>True if valid</returns>
        public async Task<bool> ValidateAccountNumberAsync(string account)
        {
            json_valid result = await PostMessage<json_valid>("{ \"action\": \"validate_account_number\", " + 
                "\"account\": \"" + account + "\" }");

            return result?.valid == 1;
        }
        #endregion

        #region Block Commands
        /// <summary>
        /// Returns the account containing block
        /// </summary>
        /// <param name="blockHash">Block's hash to find the owning account</param>
        /// <returns>Nano account number that contains the block</returns>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        public async Task<string> GetBlockAccountAsync(string blockHash)
        {
            json_account result = await PostMessage<json_account>("{ \"action\": \"block_account\", " + 
                "\"hash\": \"" + blockHash + "\" }");

            return result?.account;
        }

        /// <summary>
        /// Reports the number of blocks in the ledger by type (send, receive, open, change)
        /// </summary>
        /// <returns><see cref="BlockCountByType"/></returns>
        public async Task<BlockCountByType> CountBlockTypesAsync()
        {
            return await PostMessage<BlockCountByType>();
        }

        /// <summary>
        /// Reports the number of blocks in the ledger and unchecked synchronizing blocks
        /// </summary>
        /// <returns><see cref="BlockCount"/></returns>
        public async Task<BlockCount> CountBlocksAsync()
        {
            return await PostMessage<BlockCount>();
        }
        
        /// <summary>
        /// Returns a list of block hashes in the account chain starting at block up to count
        /// </summary>
        /// <param name="block">Start block</param>
        /// <param name="count">Number of blocks to return</param>
        /// <returns>Array of block hashes from the specified block to the specified count</returns>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public async Task<string[]> GetChainAsync(string block, UInt64 count)
        {
            json_block_list result = await PostMessage<json_block_list>("{ \"action\": \"chain\", " + 
                "\"block\": \"" + block + "\", \"count\": \"" + count + "\" }");

            return result?.blocks.ToArray();
        }

        #region TODO: Complete this
        /**
         * TODO: This should be split into multiple functions
         * Returns: char *hash, struct block block
         * 
         * Error: RPC control is disabled
         * TODO: If this really allows the private key to go over the network 
         *       then there is a MITM attack waiting to happen
         * Error: Private key or local wallet and account required
         * Error: Invalid block type
         * Error: Destination account, previous hash, current balance and 
         *        amount required
         * Error: Insufficient balance
         * Error: Representative account and previous hash required
         * Error: Previous hash and source hash required
         * Error: Representative account and source hash required
         * Error: Bad balance number
         * Error: Invalid previous hash
         * Error: Bad private key
         * Error: Wallet not found
         * Error: Wallet is locked
         * Error: Account not found in wallet
         * Error: Bad work
         * Error: Bad amount number
         * Error: Invalid source hash
         * Error: Bad destination account
         * Error: Bad representative account
         * Error: Bad account number
         * Error: Bad wallet number
         */
        //void block_create(struct rpc_server *rpc, char* type,
        //    /* optional */
        //                   char* wallet, /* optional */ char* account,
        //    /* optional */
        //                   char* representative,
        //    /* optional */
        //                   char* destination, /* optional */ char* source,
        //    /* optional */ uint128_t amount, /* optional */ uint64_t work,
        //    /* optional */
        //                   char* key, /* optional */ char* previous,
        //    /* optional */ uint128_t balance);
        public void CreateOpenBlock(string key, string account, string representative, string source, string work = null)
        {

        }

        public void CreateReceiveBlock(string wallet, string account, string source, string previousBlock, string work = null)
        {

        }

        public void CreateSendBlock(string wallet, string account, string destination, BigInteger balance, BigInteger amount, string previousBlock, string work = null)
        {

        }

        public void CreateChangeBlock(string wallet, string account, string representative, string previousBlock, string work = null)
        {
            
        }
        #endregion

        /// <summary>
        /// Publish block to the network
        /// </summary>
        /// <param name="block">Block to publish to the network</param>
        /// <returns>The new blocks hash</returns>
        /// <exception cref="BlockInvalidException"/>
        /// <exception cref="BlockWorkInvalidException"/>
        /// <exception cref="ErrorProcessingBlockException"/>
        /// <exception cref="AccountMismatchException"/>
        /// <exception cref="ForkException"/>
        /// <exception cref="NotReceiveFromSendException"/>
        /// <exception cref="BlockUnreceivableException"/>
        /// <exception cref="OverspendException"/>
        /// <exception cref="BadSignatureException"/>
        /// <exception cref="OldBlockException"/>
        /// <exception cref="GapSourceBlockException"/>
        /// <exception cref="GapPreviousBlockException"/>
        public async Task<string> ProcessBlockAsync(string block)
        {
            json_hash result = await PostMessage<json_hash>("{ \"action\": \"process\", \"block\": \"" + block + "\" } }");

            return result?.hash;
        }


        /// <summary>
        /// Retrieves a json representation of block
        /// </summary>
        /// <param name="hash">The block's hash</param>
        /// <returns>JSON representation of block</returns>
        /// <exception cref="BadHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        public async Task<string> GetBlockAsync(string hash)
        {
            json_contents result = await PostMessage<json_contents>("{ \"action\": \"block\", " + 
                "\"hash\": \"" + hash + "\" }");

            return result?.contents;
        }
        
        /// <summary>
        /// Retrieves a json representations of blocks with transaction amount and block account
        /// </summary>
        /// <param name="hashes">Block hashes to get information on</param>
        /// <param name="pending">Toggle retrieving pending blocks</param>
        /// <param name="source">Toggle retrieving block source account</param>
        /// <returns><see cref="BlockInformation"/></returns>
        /// <exception cref="BadHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        public async Task<BlockInformation[]> GetBlocksInformationAsync(string[] hashes, bool pending = false, bool source = false)
        {
            List<BlockInformation> result = new List<BlockInformation>();

            JObject resultJson = JObject.Parse(await PostMessage<string>("{ \"action\": \"blocks_info\", " +
                "\"hashes\": [ " + hashes.ToJsonString(false) + " ]" + (pending ? ", \"pending\": \"true\"" : "") +  (source ? ", \"source\": \"true\"" : "") + " }"));

            if (resultJson["blocks"] is JObject jBlocks)
            {
                foreach (JProperty jBlock in jBlocks.Properties())
                {
                    string blockHash = jBlock.Name;

                    if (jBlock.Value is JObject jBlockObject)
                    {
                        string blockAccount = jBlockObject["block_account"].ToString();
                        string contents = jBlockObject["contents"].ToString();
                        BigInteger.TryParse((jBlockObject["amount"] as JValue)?.Value as String, out BigInteger amount);
                        bool.TryParse((jBlockObject["pending"] as JValue)?.Value as String, out bool isPending);
                        string sourceAccount = jBlockObject["source_account"]?.ToString();

                        result.Add(new BlockInformation()
                        {
                            BlockHash = blockHash,
                            BlockAccount = blockAccount,
                            Contents = contents,
                            Amount = amount,
                            Pending = isPending,
                            SourceAccount = sourceAccount
                        });
                    }
                    else
                    {
                        // TODO: Handle this?
                    }
                }
            }

            return result.ToArray();
        }
        
        /// <summary>
        /// Retrieves json representations of blocks
        /// </summary>
        /// <param name="hashes">The hashes of the blocks to get</param>
        /// <returns><see cref="BlockWithHash"/></returns>
        /// <exception cref="BadHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        public async Task<BlockWithHash[]> GetBlocksAsync(string[] hashes)
        {
            List<BlockWithHash> result = new List<BlockWithHash>();

            JObject resultJson = JObject.Parse(await PostMessage<string>(""));

            if (resultJson["blocks"] is JObject jBlocks)
            {
                foreach (JProperty jBlock in jBlocks.Properties())
                {
                    string blockHash = jBlock.Name;
                    string block = jBlock.Value.ToString();

                    result.Add(new BlockWithHash()
                    {
                        Hash = blockHash,
                        Block = block
                    });
                }
            }

            return result.ToArray();
        }
        #endregion

        #region Bootstrap
        /// <summary>
        /// Initialize bootstrap to specific IP address and port
        /// </summary>
        /// <param name="address">IP address to bootstrap to</param>
        /// <param name="port">Port to bootstrap to IP address with</param>
        /// <exception cref="InvalidIPAddressException"/>
        /// <exception cref="InvalidPortException"/>
        public async Task BootstrapAsync(string address, UInt16 port)
        {
            await PostMessage("{ \"action\": \"bootstrap\", \"address\": \"" + address + "\", \"port\": \"" + port + "\" }");
        }

        /// <summary>
        /// Initialize multi-connection bootstrap to random peers
        /// </summary>
        public async Task BootstrapAnyAsync()
        {
            await PostMessage("{ \"action\": \"bootstrap_any\" }");
        }
        #endregion

        #region Delegators
        /// <summary>
        /// Returns a list of pairs of delegator names given account a representative and its balance
        /// </summary>
        /// <param name="representative">Nano representative account to get delegators of</param>
        /// <returns>List of delegator accounts and their balances</returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<Delegator[]> GetDelegatorsAsync(string representative)
        {
            List<Delegator> result = new List<Delegator>();

            JObject resultJson = JObject.Parse(await PostMessage<string>("{ \"action\": \"delegators\", \"account\": \"" + representative + "\" }"));

            if (resultJson["blocks"] is JObject jBlocks)
            {
                foreach (JProperty jBlock in jBlocks.Properties())
                {
                    string account = jBlock.Name;
                    BigInteger.TryParse((jBlock.Value as JValue)?.Value as String, out BigInteger amount);

                    result.Add(new Delegator()
                    {
                        Account = account,
                        Balance = amount
                    });
                }
            }

            return result.ToArray();
        }
        
        /// <summary>
        /// Get number of delegators for a specific representative account
        /// </summary>
        /// <param name="representative">Nano representative account to count the delegators of</param>
        /// <returns>Count of the representative's delegators</returns>
        /// <exception cref="BadAccountNumberException"/>
        public async Task<UInt64> GetDelegatorsCountAsync(string representative)
        {
            json_count result = await PostMessage<json_count>("{ \"action\": \"delegators\", \"account\": \"" + representative + "\" }");

            return result.count;
        }
        #endregion

        #region Frontiers
        /// <summary>
        /// Returns a list of pairs of account and block hash representing the head block starting at account up to count
        /// </summary>
        /// <param name="account">Starting Nano account</param>
        /// <param name="count">Number of frontiers to receive</param>
        /// <returns>List of <see cref="Frontier"/></returns>
        /// <exception cref="InvalidStartingAccountException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public Frontier[] GetFrontiers(string account, UInt64 count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reports the number of accounts in the ledger
        /// </summary>
        /// <returns>Number of Nano accounts that exist</returns>
        public UInt64 CountFrontiers()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Keys
        /// <summary>
        /// Derive deterministic keypair from seed based on index
        /// </summary>
        /// <param name="seed">Seed for key generation</param>
        /// <param name="index">Index to use for key creation</param>
        /// <returns><see cref="KeyPair"/></returns>
        /// <exception cref="BadSeedException"/>
        /// <exception cref="InvalidIndexException"/>
        public KeyPair GenerateDeterministicKey(string seed, UInt64 index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates an adhoc random keypair
        /// </summary>
        /// <returns><see cref="KeyPair"/></returns>
        public KeyPair CreateRamdomKey()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Derive public key and account number from private key
        /// </summary>
        /// <param name="key">Private key to derive public key and account from</param>
        /// <returns><see cref="KeyPair"/></returns>
        /// <exception cref="BadPrivateKeyException"/>
        public KeyPair ExpandKey(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Ledger
        /// <summary>
        /// Reports send/receive information for a chain of blocks
        /// </summary>
        /// <param name="hash">Starting block hash</param>
        /// <param name="count">Number of transactions to return</param>
        /// <returns><see cref="Transaction"/></returns>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public Transaction[] GetHistory(string hash, UInt64 count)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns frontier, open block, change representative block, balance, last modified timestamp from local database & block count starting at account up to count
        /// Optional "representative", "weight", "pending":
        /// Additionally returns representative, voting weight, pending balance for each account
        /// Optional "sorting":
        /// Additional sorting accounts in descending order
        /// </summary>
        /// <param name="account">Starting Nano account</param>
        /// <param name="count">Number of ledger accounts to return</param>
        /// <param name="representative">Toggle returning representative</param>
        /// <param name="weight">Toggle returning weight</param>
        /// <param name="pending">Toggle returning pending balance</param>
        /// <param name="sorting">Toggle descending sort</param>
        /// <returns><see cref="LedgerAccount"/></returns>
        /// <exception cref="RpcControlDisabledException"/>
        public LedgerAccount[] GetLedger(string account, UInt64 count, bool representative = false, bool weight = false, bool pending = false, bool sorting = false)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a list of block hashes in the account chain ending at block up to count
        /// </summary>
        /// <param name="block">Block to start at</param>
        /// <param name="count">Number of blocks prior to start block return</param>
        /// <returns>List of block hashes in the account up to count</returns>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public string[] GetSuccessors(string block, UInt64 count)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Network
        // TODO: Is this supposed to be RAW instead of rai?
        /// <summary>
        /// Returns how many rai are in the public supply
        /// </summary>
        /// <returns>Number of rai in the public supply</returns>
        public BigInteger GetAvailableSupply()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Tells the node to send a keepalive packet to address:port
        /// </summary>
        /// <param name="address">IP address to send keepalive packet to</param>
        /// <param name="port">Port to send keepalive packet to on the IP address</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="InvalidPortException"/>
        public void SendKeepAlive(string address, UInt16 port)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Rebroadcast blocks starting at hash to the network
        /// Optional "sources":
        /// Additionally rebroadcast source chain blocks for receive/open up to sources depth
        /// Optional "destinations"
        /// Additionally rebroadcast destination chain blocks from receive up to destinations depth
        /// </summary>
        /// <param name="hash">Starting block hash</param>
        /// <param name="count"></param>
        /// <param name="sources"></param>
        /// <param name="destinations"></param>
        /// <returns>Republished block hashes</returns>
        /// <exception cref="BadHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        public string[] Republish(string hash, UInt64 count = 0, UInt64 sources = 0, UInt64 destinations = 0)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Node
        /// <summary>
        /// Returns version information for RPC, Store & Node (Major & Minor version)
        /// </summary>
        /// <returns><see cref="NanoVersion"/></returns>
        public NanoVersion GetNanoVersion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to safely shutdown node
        /// </summary>
        /// <exception cref="RpcControlDisabledException"/>
        public void StopNode()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Payments
        /// <summary>
        /// Begin a new payment session. Searches wallet for an account that's marked as available and 
        /// has a 0 balance. If one is found, the account number is returned and is marked as unavailable. 
        /// If no account is found, a new account is created, placed in the wallet, and returned.
        /// </summary>
        /// <param name="wallet">Wallet to search for/create an account in</param>
        /// <returns>The account used for the payment</returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="WalletLockedException"/>
        /// <exception cref="UnableToCreateTransactionAccountException"/>
        public string BeginPayment(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// End a payment session. Marks the account as available for use in a payment session
        /// </summary>
        /// <param name="wallet">Wallet used for payment</param>
        /// <param name="account">Account used for payment</param>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="InvalidAccountNumberException"/>
        /// <exception cref="AccountNotFoundInWalletException"/>
        /// <exception cref="AccountBalanceNonZeroException"/>
        public void EndPayment(string wallet, string account)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Marks all accounts in wallet as available for being used as a payment session.
        /// </summary>
        /// <param name="wallet">Wallet for the payment</param>
        /// <returns>Status</returns>
        /// <exception cref="BadWalletNumberException"/>
        public string InitPayment(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Wait for payment of 'amount' to arrive in 'account' or until 'timeout' milliseconds have elapsed
        /// </summary>
        /// <param name="account">Account to receive the payment in</param>
        /// <param name="amount">Payment amount</param>
        /// <param name="timeout">Time in milliseconds to wait</param>
        /// <returns></returns>
        public string WaitPayment(string account, BigInteger amount, UInt64 timeout)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Peers
        /// <summary>
        /// Add specific IP address and port as work peer for node until restart
        /// </summary>
        /// <param name="address">IP address for peer</param>
        /// <param name="port">Port for peer's IP address</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="InvalidIPAddressException"/>
        /// <exception cref="InvalidPortException"/>
        public void AddWorkPeer(string address, UInt16 port)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Clear work peers node list until restart
        /// </summary>
        /// <exception cref="RpcControlDisabledException"/>
        public void ClearWorkPeers()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of pairs of peer IPv6:port and its node network version
        /// </summary>
        /// <returns>List of <see cref="Peer"/></returns>
        public Peer[] GetPeers()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a list of pairs of work peer IPv6:port
        /// </summary>
        /// <returns>List of <see cref="Peer"/></returns>
        public Peer[] GetWorkPeers()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Pending
        /// <summary>
        /// Returns a list of block hashes which have not yet been received by this account.
        /// Optional "threshold":
        /// Returns a list of pending block hashes with amount more or equal to threshold
        /// Optional "source":
        /// Returns a list of pending block hashes with amount and source accounts
        /// </summary>
        /// <param name="account">Nano account to get pending transactions from</param>
        /// <param name="count">Number of pending transactions to receive</param>
        /// <param name="threshold">Return only transactions with a balance greater than or equal to this value</param>
        /// <param name="source">Return source account?</param>
        /// <returns>List of <see cref="PendingTransaction"/></returns>
        /// <exception cref="BadAccountNumberException"/>
        public PendingTransaction[] GetPending(string account, UInt64 count, BigInteger threshold = default(BigInteger), bool source = false)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Check whether block is pending by hash
        /// </summary>
        /// <param name="hash">Hash to check if it's pending</param>
        /// <returns>True - block is pending, false - block is not pending</returns>
        /// <exception cref="BadHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        public bool CheckPendingExists(string hash)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Tells the node to look for pending blocks for any account in wallet
        /// </summary>
        /// <param name="wallet">Wallet to search for pending blocks</param>
        /// <returns>Started</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="WalletNotFoundException"/>
        public bool SearchPending(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Tells the node to look for pending blocks for any account in all available wallets
        /// </summary>
        /// <exception cref="RpcControlDisabledException"/>
        public void SearchPendingAllWallets()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Receiving
        /// <summary>
        /// Receive pending block for account in wallet
        /// </summary>
        /// <param name="wallet">Wallet containing the pending block</param>
        /// <param name="account">Account containing pending block</param>
        /// <param name="block">Block hash to receive</param>
        /// <param name="work">Optional precomputed work</param>
        /// <returns>New block hash</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundInWalletException"/>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="BlockNotFoundException"/>
        /// <exception cref="BlockNotReceivableException"/>
        public string Receive(string wallet, string account, string block, string work = null)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns receive minimum for node
        /// </summary>
        /// <returns>Minimum the node will allow to be received</returns>
        /// <exception cref="RpcControlDisabledException"/>
        public BigInteger GetMinimumReceivable()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Set amount as new receive minimum for node until restart
        /// </summary>
        /// <param name="amount">Minimum the node will allow to be received</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadAmountNumberException"
        public void SetMinimumReceivable(BigInteger amount)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Representatives
        /// <summary>
        /// Returns a list of pairs of representative and its voting weight
        /// </summary>
        /// <param name="count">Optionally only return representatives up to count</param>
        /// <param name="sorting">Sort representatives in descending order</param>
        /// <returns><see cref="Representative"/></returns>
        public Representative[] GetRepresentatives(UInt64 count = 0, bool sorting = false)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns the default representative for wallet
        /// </summary>
        /// <param name="wallet">Wallet to get the representative for</param>
        /// <returns>Representative account number</returns>
        /// TODO: The node returns "bad account number" but shouldn't it be "bad wallet number"?
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public string GetWalletRepresentative(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Sets the default representative for wallet
        /// </summary>
        /// <param name="wallet">Wallet to set the representative for</param>
        /// <param name="representative">Representative to use for the wallet</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// TODO: Ditto to the above function's TODO
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="InvalidAccountNumberException"/>
        public void SetWalletRepresentative(string wallet, string representative)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Sending
        /// <summary>
        /// Send amount from source in wallet to destination
        /// </summary>
        /// <param name="wallet">Nano wallet to send from</param>
        /// <param name="source">Nano account in wallet to send from</param>
        /// <param name="destination">Nano account to send to</param>
        /// <param name="amount">Amount to send</param>
        /// <param name="work">Optional precomputed work to use</param>
        /// <returns>Send block hash</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadSourceNumberException"/>
        /// <exception cref="BadDestinationNumberException"/>
        /// <exception cref="BadAmountNumberException"/>
        /// <exception cref="InsufficientBalanceException"/>
        /// <exception cref="InvalidWorkException"/>
        /// <exception cref="AccountNotFoundException"/>
        /// <exception cref="BadWorkException"/>
        public string Send(string wallet, string source, string destination, BigInteger amount, string work = null)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Unchecked
        /// <summary>
        /// Clear unchecked synchronizing blocks
        /// </summary>
        /// <exception cref="RpcControlDisabledException"/>
        public void ClearUnchecked()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Retrieves a json representation of unchecked synchronizing block by hash
        /// </summary>
        /// <param name="hash">Hash for unchecked block to return</param>
        /// <returns>JSON representation of unchecked block</returns>
        /// <exception cref="BadHashException"/>
        /// <exception cref="UncheckedBlockNotFoundException"/>
        public string GetUnchecked(string hash)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Retrieves unchecked database keys, blocks hashes & a json representations of unchecked pending blocks starting from key up to count
        /// </summary>
        /// <param name="key">Starting key hash</param>
        /// <param name="count">Number of blocks to return</param>
        /// <returns><see cref="UncheckedBlockWithKey"/></returns>
        /// <exception cref="BadKeyHashException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public UncheckedBlockWithKey[] GetUncheckedKeys(string key, UInt64 count)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a list of pairs of unchecked synchronizing block hash and its json representation up to count
        /// </summary>
        /// <param name="count">Number of unchecked blocks to return</param>
        /// <returns><see cref="BlockWithHash"/></returns>
        public BlockWithHash[] GetUnchecked(UInt64 count)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Wallet
        /// <summary>
        /// Returns how many Nano is owned and how many have not yet been received by all accounts in wallet
        /// Optional "threshold":
        /// Returns wallet accounts balances more or equal to threshold
        /// </summary>
        /// <param name="wallet">Nano wallet to get the accounts and balances from</param>
        /// <param name="threshold">Set a minimum balance for wallets to retrieve</param>
        /// <returns><see cref="AccountBalance"/></returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadThresholdNumberException"/>
        public AccountBalance[] GetWalletBalances(string wallet, /* optional */ BigInteger threshold)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Add an adhoc private key key to wallet
        /// </summary>
        /// <param name="key">Private key for wallet</param>
        /// <param name="wallet">Wallet to add</param>
        /// <param name="work">Toggle work after creating account</param>
        /// <returns>Account string</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadPrivateKeyException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="WalletLockedException"/>
        public string AddWallet(string key, string wallet, bool work = true)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Changes the password for wallet to password
        /// </summary>
        /// <param name="wallet">Wallet to change the password to</param>
        /// <param name="password">New password</param>
        /// <returns>Success/Failure</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public bool ChangeWalletPassword(string wallet, string password)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Changes seed for wallet to seed
        /// </summary>
        /// <param name="wallet">Wallet to change the seed for</param>
        /// <param name="seed">Seed for the wallet</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadSeedException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="WalletLockedException"/>
        public void ChangeWalletSeed(string wallet, string seed)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Check whether wallet contains account
        /// </summary>
        /// <param name="wallet">Wallet to check for the account in</param>
        /// <param name="account">Account to check for</param>
        /// <returns>True/false if wallet contains account</returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public bool WalletContains(string wallet, string account)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Creates a new random wallet id
        /// </summary>
        /// <returns>New wallet id</returns>
        /// <exception cref="RpcControlDisabledException"/>
        public string CreateWallet()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Destroys wallet and all contained accounts
        /// </summary>
        /// <param name="wallet">Wallet to destroy</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public void DestroyWallet(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Return a json representation of wallet
        /// </summary>
        /// <param name="wallet">Wallet to export</param>
        /// <returns>JSON representation of wallet</returns>
        /// TODO: This should be "BadWalletNumberException" but the node returns bad account
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public string ExportWallet(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a list of pairs of account and block hash representing the head block starting for accounts of wallet
        /// </summary>
        /// <param name="wallet">Wallet to get frontiers from</param>
        /// <returns><see cref="Frontier"/></returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public Frontier[] GetWalletFrontiers(string wallet)
        {
            throw new NotImplementedException();
        }

        // TODO: wallet_locked -> password_valid
        
        /// <summary>
        /// Enters the password in to wallet
        /// </summary>
        /// <param name="wallet">Wallet to try to unlock</param>
        /// <param name="password">Password to use for unlocking</param>
        /// <returns>Success/failure</returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public bool EnterPassword(string wallet, string password)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a list of block hashes which have not yet been received by accounts in this wallet
        /// Optional "threshold":
        /// Returns a list of pending block hashes with amount more or equal to threshold
        /// Optional "source":
        /// Returns a list of pending block hashes with amount and source accounts
        /// </summary>
        /// <param name="wallet">Wallet to get pending transactions from</param>
        /// <param name="count">Number of pending to retrieve</param>
        /// <param name="threshold">Minimum balance for the pending transaction to receive</param>
        /// <param name="source">Toggle returning source account</param>
        /// <returns><see cref="PendingTransaction"/></returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadThresholdNumberException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public PendingTransaction[] GetWalletPending(string wallet, UInt64 count, BigInteger threshold = default(BigInteger), bool source = false)
        {
            throw new NotImplementedException();
        }

        // wallet_representative_set is found in representatives.h
        // wallet_representative is found in representatives.h
        
        /// <summary>
        /// Rebroadcast blocks for accounts from wallet starting at frontier down to count to the network
        /// </summary>
        /// <param name="wallet">Wallet to republish blocks from</param>
        /// <param name="count">Number of blocks to republish</param>
        /// <returns>Array of republished block hashes</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="InvalidCountLimitException"/>
        public string[] WalletRepublish(string wallet, UInt64 count)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns the sum of all accounts balances in wallet
        /// </summary>
        /// <param name="wallet">Nano wallet to get total balance for</param>
        /// <returns><see cref="Balance"/></returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public Balance GetWalletTotalBalance(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Checks whether the password entered for wallet is valid
        /// </summary>
        /// <param name="wallet">Nano wallet to check the password for</param>
        /// <returns>Valid/invalid</returns>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public bool CheckPasswordValid(string wallet)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Returns a list of pairs of account and work from wallet
        /// </summary>
        /// <param name="wallet">Wallet to get work from</param>
        /// <returns><see cref="AccountWork"/></returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        public AccountWork[] GetWalletWork(string wallet)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Work
        /// <summary>
        /// Stop generating work for block
        /// </summary>
        /// <param name="hash">Block to stop generating work for</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="InvalidBlockHashException"/>
        public void CancelWork(string hash)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Generates work for block
        /// </summary>
        /// <param name="hash">Block hash to generate work for</param>
        /// <returns>Generated work for the block</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="WorkCancelledException"/>
        public string GenerateWork(string hash)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Retrieves work for account in wallet
        /// </summary>
        /// <param name="wallet">Wallet containing the account</param>
        /// <param name="account">Account to get the work from</param>
        /// <returns>The work for the provided account</returns>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundInWalletException"/>
        public string GetWork(string wallet, string account)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Set work for account in wallet
        /// </summary>
        /// <param name="wallet">Wallet containing the account</param>
        /// <param name="account">Account to get the work from</param>
        /// <param name="work">Work for the account</param>
        /// <exception cref="RpcControlDisabledException"/>
        /// <exception cref="BadWalletNumberException"/>
        /// <exception cref="WalletNotFoundException"/>
        /// <exception cref="BadAccountNumberException"/>
        /// <exception cref="AccountNotFoundInWalletException"/>
        /// <exception cref="BadWorkException"/>
        public void SetWork(string wallet, string account, string work)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Check whether work is valid for block
        /// </summary>
        /// <param name="hash">Block hash the work is for</param>
        /// <param name="work">Computed work for the block</param>
        /// <returns>Valid/invalid</returns>
        /// <exception cref="InvalidBlockHashException"/>
        /// <exception cref="BadWorkException"/>
        public bool ValidateWork(string hash, string work)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
