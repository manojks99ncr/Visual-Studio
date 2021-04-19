using EHContactGrpcService.Protos;
using EHContactGrpcService.Repositories;
using Grpc.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EHContactGrpcService.Services
{
    public class EHContactService : EHContactGrpcService.Protos.EHContact.EHContactBase
    {
        private readonly ILogger<EHContactService> logger;
        private readonly ContactRepository repository;

        public EHContactService(ILogger<EHContactService> logger, ContactRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [EnableCors("MyPolicy")]
        public override Task<ContactModel> CreateNewContact(ContactModel request, ServerCallContext context)
        {
            ContactModel response = repository.AddContact(request);

            return Task.FromResult(response);
        }

        [EnableCors("MyPolicy")]
        public override Task<ContactsResponse> GetAllContacts(GetAllRequest request, ServerCallContext context)
        {
            ContactsResponse response = new ContactsResponse();
            foreach (var contact in repository.Contacts)
            {
                response.Contact.Add(contact);
            }
            return Task.FromResult(response);
        }

        [EnableCors("MyPolicy")]
        public override Task<ContactModel> UpdateContact(ContactModel request, ServerCallContext context)
        {
            ContactModel updateContact = repository.FindContact(request.ContactID);
            if (updateContact == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Contact with ID={request.ContactID} is not found."));
            }
            updateContact.FirstName = request.FirstName;
            updateContact.LastName = request.LastName;
            updateContact.Address = request.Address;
            updateContact.City = request.City;
            updateContact.Country = request.Country;
            updateContact.Zipcode = request.Zipcode;

            return Task.FromResult(updateContact);
        }

        [EnableCors("MyPolicy")]
        public override Task<GenericResponseMessage> DeleteContact(DeleteContactRequest request, ServerCallContext context)
        {
            ContactModel deleteContact = repository.FindContact(request.ContactID);

            if (deleteContact == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Contact with ID={request.ContactID} is not found."));
            }

            repository.Contacts.Remove(deleteContact);

            return Task.FromResult(new GenericResponseMessage { Message = "Contact is successfuly deleted" });
        }

        [EnableCors("MyPolicy")]
        public override Task<ContactModel> GetContact(GetContactRequest request, ServerCallContext context)
        {
            ContactModel contactResponse = repository.FindContact(request.ContactID);

            if (contactResponse == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Contact with ID={request.ContactID} is not found."));
            }

            return Task.FromResult(contactResponse);
        }
    }
}
