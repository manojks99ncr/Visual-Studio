using EHContactClient.Data;
using EHContactGrpcService.Protos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EHContactClient.CodeFiles
{
    public class ContactOperation
    {
        public bool ShowModel = false;
        public bool ShowAlert = false;
        public bool ShowModeletePopup = false;
        public string OperationStatusText = "";
        public string PopupTitle = "";
        public ContactData contactData = null;
        public string ActionText = "";
        ContactBookClient client = null;

        public IList<ContactModel> contactModel { get; set; }
        public string DeleteItemId { get; set; }

        public ContactOperation()
        {
            client = new ContactBookClient("https://localhost:5001");
        }

        public async Task GetContactList()
        {
            await Task.Delay(1000);
            ContactsResponse contactsResponse = await client.ShowAllContacts();
            contactModel = contactsResponse.Contact;
        }


        public async Task ShowEditForm(int Id)
        {
            PopupTitle = "Contact Edit";
            ActionText = "Update";
            ContactModel cm = await client.GetContact(Id);
            contactData = new ContactData();
            contactData.Id = cm.ContactID;
            contactData.FirstName = cm.FirstName;
            contactData.LastName = cm.LastName;
            contactData.PhoneNumber = cm.PhoneNumbers[0].Number;

            ShowModel = true;
        }

        public void ShowAddpopup()
        {
            contactData = new ContactData() { FirstName = "", LastName = "", PhoneNumber = "" };
            PopupTitle = "Contact Data";
            ActionText = "Add";
            ShowModel = true;
        }
        public void ShowDeletePopup(string Id)
        {
            DeleteItemId = Id;
            ShowModeletePopup = true;
        }

        public async Task PostData()
        {
            bool status = false;
            ContactModel cm = new ContactModel();
            cm.ContactID = contactData.Id;
            cm.FirstName = contactData.FirstName;
            cm.LastName = contactData.LastName;
            cm.PhoneNumbers.Add(new PhoneNumberModel
            {
                Number = contactData.PhoneNumber
            });

            if (contactData.Id > 0 && contactData.Id<=20)
            {
                status = await client.UpdateContact(cm);

            }
            else
            {
                status = await client.CreateNewContact(cm);
            }
            await Reload(status);
        }

        public async Task DeleteData()
        {

            var operationStatus = await client.DeleteContact(Convert.ToInt32(DeleteItemId));
            await Reload(operationStatus);
        }

        protected async Task Reload(bool status)
        {
            ShowModeletePopup = false;
            ShowModel = false;
            await this.GetContactList();
            ShowAlert = true;
            if (status)
            {
                OperationStatusText = "Processed Successfully !! ";
            }
            else
            {
                OperationStatusText = "Error Occured  ";
            }

        }

        public void DismissPopup()
        {
            ShowModel = false;
            ShowAlert = false;
            ShowModeletePopup = false;
        }

    }
}
