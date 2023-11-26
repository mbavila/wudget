// <copyright file="Messages.cs" company="Alliance Software Inc">
// Copyright (c) Alliance Software Inc. All rights reserved.
// </copyright>

namespace BaseCodeDotNetCore.Domain
{
    public static class Messages
    {
        public static class HttpErrorMessages
        {
            public const string Error500 = "Internal server error";
        }

        public static class ExceptionMessages
        {
            public const string ExceptionGeneric = "Something went wrong inside the {0} action: {1}";
            public const string DBContextNull = "DB Context is not initialized.";
        }

        public static class InputFieldValidation
        {
            public const string ValueRequired = "{0} is required.";
        }

        public static class EmployeeMessages
        {
            public const string EmployeeExist = "There is an existing employee named {0} in the database.";
            public const string EmployeeSaved = "Employee {0} was saved successfully.";
            public const string EmployeeUpdated = "Employee {0} was updated successfully.";
            public const string EmployeeDeleted = "Employee {0} was deleted successfully.";
            public const string EmployeeDoesNotExist = "The employee record that you are trying to update does not exist in the database.";
        }

        public static class SubcategoryMessages
        {
            public const string SubCategoryExist = "There is an existing subcategory named {0} in the database.";
            public const string SubCategorySaved = "SubCategory {0} was saved successfully.";
            public const string SubCategoryUpdated = "SubCategory {0} was updated successfully.";
            public const string SubCategoryDeleted = "SubCategory {0} was deleted successfully.";
            public const string SubCategoryDoesNotExist = "The subcategory record that you are trying to update does not exist in the database.";
        }

        public static class BudgetMessages
        {
            public const string BudgetExist = "There is an existing budget named {0} in the database.";
            public const string BudgetSaved = "Budget was saved successfully.";
            public const string BudgetUpdated = "Budget was updated successfully.";
            public const string BudgetDeleted = "Budget was deleted successfully.";
            public const string BudgetDoesNotExist = "The budget record that you are trying to update does not exist in the database.";
        }

        public static class TransactionMessages
        {
            public const string TransactionSaved = "Transaction was saved successfully.";
            public const string TransactionUpdated = "Transaction was updated successfully.";
            public const string TransactionDeleted = "Transaction was deleted successfully.";
            public const string TransactionDoesNotExist = "The transaction record that you are trying to update does not exist in the database.";
        }

        public static class CollectionMessages
        {
            public const string CollectionExist = "There is an existing collection named {0} in the database.";
            public const string CollectionSaved = "Collection {0} was saved successfully.";
            public const string CollectionUpdated = "Collection {0} was updated successfully.";
            public const string CollectionDeleted = "Collection {0} was deleted successfully.";
            public const string CollectionDoesNotExist = "The Collection record that you are trying to update does not exist in the database.";
        }

        public static class LoginMessages
        {
            public const string InvalidRequestDetails = "Invalid request. Please provide user credentials.";
            public const string InvalidClientId = "Invalid client id.";
            public const string IncorrectCredentials = "Incorrect username or password.";
        }
    }
}
