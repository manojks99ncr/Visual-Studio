using EHContactGrpcService.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EHContactClient
{
    /// <summary>
    /// Implementation of contactbook grpc client
    /// </summary>
    public class ContactBookClient
    {
        /// <summary>
        /// Init channel and client
        /// </summary>
        /// <param name="serverUrl"></param>
        public ContactBookClient(string serverUrl)
        {
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

            contactBookChannel = GrpcChannel.ForAddress(serverUrl, new GrpcChannelOptions
            {
                HttpClient = new HttpClient(handler)
            });
            contactBookClient = new EHContact.EHContactClient(contactBookChannel);

        }

        private GrpcChannel contactBookChannel;
        private EHContact.EHContactClient contactBookClient;

        /// <summary>
        /// Getting all contacts from rpc
        /// </summary>
        /// <returns></returns>
        internal async Task<ContactsResponse> ShowAllContacts()
        {
            ContactsResponse contacts = null;
            try
            {
                // Execute rpc
                // Request is empty object so it is just created on the same line
                 contacts = await contactBookClient.GetAllContactsAsync(new GetAllRequest());

                if (contacts == null && contacts.Contact == null && contacts.Contact.Count > 0)
                {
                    Console.WriteLine("There are no contacts");
                }
            }
            catch (RpcException rpcException)
            {
                Console.WriteLine("There was an error communicating with gRPC server");
                Console.WriteLine($"Code: {rpcException.StatusCode}, Status: {rpcException.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return contacts;
        }

        internal async Task<ContactModel> GetContact(int contactID)
        {
            ContactModel contact = await contactBookClient.GetContactAsync(new GetContactRequest { ContactID = contactID });
            return contact;
        }


        internal async Task<bool> CreateNewContact(ContactModel newContact)
        {
            bool status = false;
          
                try
                {
                    var response = await contactBookClient.CreateNewContactAsync(newContact);
                    status = true;
                }
                catch (RpcException rpcException)
                {
                    Console.WriteLine("There was an error communicating with gRPC server");
                    Console.WriteLine($"Code: {rpcException.StatusCode}, Status: {rpcException.Status}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            return status;
        }

       
        internal async Task<bool> DeleteContact(int contactID)
        {
            bool status = false;

            DeleteContactRequest deleteRequest = new DeleteContactRequest();
            deleteRequest.ContactID = contactID;

            try
            {
                ContactModel contact = await contactBookClient.GetContactAsync(new GetContactRequest { ContactID = contactID });
                    try
                    {
                        var response = await contactBookClient.DeleteContactAsync(deleteRequest);
                        status = true;
                    }
                    catch (RpcException rpcException)
                    {
                        Console.WriteLine("There was an error communicating with gRPC server");
                        Console.WriteLine($"Code: {rpcException.StatusCode}, Status: {rpcException.Status}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
            }
            catch (RpcException rpcException)
            {
                if (rpcException.StatusCode == StatusCode.NotFound)
                {
                    Console.WriteLine($"Could not find contact with ID={deleteRequest.ContactID}");
                }
                else
                {
                    Console.WriteLine("There was an error communicating with gRPC server");
                    Console.WriteLine($"Code: {rpcException.StatusCode}, Status: {rpcException.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return status;
        }


        internal async Task<bool> UpdateContact(ContactModel newContact)
        {
            bool status=false;

            try
            {
                try
                {
                    var response = await contactBookClient.UpdateContactAsync(newContact);
                    status = true;

                }
                catch (RpcException rpcException)
                {
                    Console.WriteLine("There was an error communicating with gRPC server");
                    Console.WriteLine($"Code: {rpcException.StatusCode}, Status: {rpcException.Status}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            catch (RpcException rpcException)
            {
                if (rpcException.StatusCode == StatusCode.NotFound)
                {
                    Console.WriteLine($"Could not find contact with ID={newContact.ContactID}");
                }
                else
                {
                    Console.WriteLine("There was an error communicating with gRPC server");
                    Console.WriteLine($"Code: {rpcException.StatusCode}, Status: {rpcException.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return status;
        }

       
    }
}
