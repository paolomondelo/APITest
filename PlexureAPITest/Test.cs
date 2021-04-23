using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlexureAPITest
{
    [TestFixture]
    public class Test
    {
        Service service;


        [OneTimeSetUp]
        public void Setup()
        {
            service = new Service();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (service != null)
            {
                service.Dispose();
                service = null;
            }
        }

        [Test]
        public void TEST_001_Login_Success_With_Valid_User_And_Assert_Username_ID_Token()
        //TEST_001
        //GIVEN_a_user_logs_in_the_correct_credentials
        //WHEN_a_user_successfully_logs_in
        //THEN_values_should_be_STATUSCODE=OK_&_USERNAME=TESTER_&_USERID=1_&_ACCESSTOKEN=37cb9e58-99db-423c-9da5-42d5627614c5
        {
            var response = service.Login("Tester", "Plexure123");


            response.Expect(HttpStatusCode.OK);
            Assert.AreEqual(response.Entity.UserName, "Tester");
            Assert.AreEqual(response.Entity.UserId, 1);
            Assert.AreEqual(response.Entity.AccessToken, "37cb9e58-99db-423c-9da5-42d5627614c5");

        }

        [Test]
        public void TEST_002_Get_Points_For_Logged_In_User()
         //TEST_002
         //GIVEN_a_user_logs_in_using_the_correct_credentials
         //WHEN_a_user_logs_in_successfully
         //THEN_it_should_successfully_invoke_the_points_endpoint
        {
            service.addToken();
            var points = service.GetPoints();

        }

        [Test]
        public void TEST_003_Purchase_Product()
        //TEST_003
        //GIVEN_a_user_logs_in_using_the_correct_credentials
        //WHEN_a_user_enters_productid=1_as_his_productid
        //THEN_it_should_successfully_invoke_the_purchase_endpoint
        {
            int productId = 1;
            service.Purchase(productId);
        }

        [Test]
        public void TEST_004_Login_Unsuccessful_Response_Due_To_Incorrect_Unmatching_Credentials()
        //TEST_004
        //GIVEN_a_user_logs_in_using_incorrect_credentials
        //WHEN_a_user_logs_in
        //THEN_response_should_be_StatusCOde=Unauthorized_Due_To_Incorrect_Unmatching_Credential
        {
            var response = service.Login("WrongUser", "WrongPword");
            response.Expect(HttpStatusCode.Unauthorized);


        }
        [Test]
        public void TEST_005_Login_Unsuccessful_Response_Due_To_Missing_Username_Password()
        //TEST_005
        //GIVEN_a_user_logs_in_using_empty_credentials
        //WHEN_a_user_logs_in
        //THEN_response_should_be_StatusCOde=BadRequest_Due_To_Missing_Username_&_Password
        {
            var response = service.Login("", "");
            response.Expect(HttpStatusCode.BadRequest);
            


        }

        [Test]
        public void TEST_006_Points_Successful_Response()
        //TEST_006
        //GIVEN_a_user_logs_in_using_the_correct_credentials
        //WHEN_a_user_enters_productid=1_as_his_productid
        //THEN_it_should_successfully_invoke_the_points_endpoint_&_should_have_statuscode=Accepted

        //response code 202 = Accepted
        {


            var response = service.GetPoints(1);
            response.Expect(HttpStatusCode.Accepted);
          

        }

        [Test]
        public void TEST_007_Points_UnSuccessful_Response()
        //TEST_007
        //GIVEN_a_user_logs_in_using_the_correct_credentials
        //WHEN_a_user_enters_an_invalid_product_id_as_his_productid
        //THEN_it_should_successfully_invoke_the_points_endpoint_&_should_have_a_statuscode=Unauthorized

        //response code 401=Unauthorized
        {

            //  var response = service.GetPoints();
            // response.Expect(HttpStatusCode.Accepted);

            service.addToken();
            var response = service.GetPoints(7777777);
            response.Expect(HttpStatusCode.Accepted);
            Assert.AreEqual(response.Entity.UserId, 1);
            Assert.GreaterOrEqual(response.Entity.Value, 100);


        }

        [Test]
        public void TEST_008_Purchase_Success_And_Assert_Message_Purchase_Completed()
        //TEST_008
        //GIVEN_a_user_logs_in_using_the_correct_credentials
        //WHEN_a_user_enters_a_valid_product_id_as_his_productid
        //THEN_it_should_successfully_invoke_the_purchase_endpoint_&_should_have_statuscode=Accepted_&_Message=Purchase_completed

        //response code 202 = Accepted

        {

            int productId = 1;

            //service.Purchase(productId).Expect(HttpStatusCode.Accepted);
            var response = service.Purchase(productId);
            response.Expect(HttpStatusCode.Accepted);
            Assert.AreEqual(response.Entity.Message, "Purchase completed.");



        }

        [Test]
        public void TEST_009_Purchase_Unsuccessful_And_Assert_Invalid_Productid_Error_Message_And_StatusCode_Bad_Request()
        //TEST_009
        //GIVEN_a_user_logs_in_using_the_correct_credentials
        //WHEN_a_user_enters_an_invalid_product_id_as_his_productid
        //THEN_it_should_successfully_invoke_the_purchase_endpoint_&_should_have_statuscode=BadRequest_&_Message=Error: Invalid product id

        //response code 400=BadRequest
        {
            int productId = 999;

            //service.Purchase(productId).Expect(HttpStatusCode.BadRequest);
            var response = service.Purchase(productId);
            response.Expect(HttpStatusCode.BadRequest);
            Assert.AreEqual(response.Error, "\"Error: Invalid product id\"");



        }
        [Test]
        public void TEST_010_Get_Points_Successful_Assert_Response_For_Points_and_UserID()
        //TEST_010
        //GIVEN_a_user_logs_in_using_the_correct_credentials
        //WHEN_a_user_enters_a_valid_product_id_as_his_productid
        //THEN_it_should_successfully_invoke_the_points_endpoint_&_should_have_statuscode=Accepted_&_UserId=1&Value>=100&Value_is_present

        //response code 202 = Accepted
        //get user_id and validate
        //get value and validate
        {


            service.addToken();
            var response = service.GetPoints(1);
            response.Expect(HttpStatusCode.Accepted);
            Assert.AreEqual(response.Entity.UserId, 1);
            Assert.GreaterOrEqual(response.Entity.Value, 100);




        }



    }
}
