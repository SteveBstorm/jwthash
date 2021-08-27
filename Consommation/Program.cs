using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Consommation
{
    class Program
    {
        public static string BaseAddress = "http://localhost:60878/api/";

        static void Main(string[] args)
        {
            BaseRequester requester = new BaseRequester(BaseAddress);
            requester.Login("test@test.com", "Test1234=");

            string token = requester.Token;

            JwtSecurityToken jwt = new JwtSecurityToken(token);

            IEnumerable<Claim> claims = jwt.Claims;

            foreach (Claim item in claims)
            {
                Console.WriteLine(item.Type + " - " + item.Value);
            }

            List<User> lu;

            lu = requester.Get<List<User>>("user");

            foreach (User item in lu)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Email);
                Console.WriteLine(item.IsAdmin);
            }

            Console.WriteLine("------------");
            Console.WriteLine("------List de messages------");
            Console.WriteLine("------------");

            Message m = new Message { Title = "Titre du message", Content = "lorem ipsum et le reste du blabla" };

            requester.Post<Message>("message", m);

            List<Message> lm;
            lm = requester.Get<List<Message>>("message");

            foreach (Message item in lm)
            {
                Console.WriteLine("Titre : "+ item.Title);
                Console.WriteLine("Contenu : "+ item.Content);
            }

            Message m1 = requester.Get<Message>("message/" + lm[0].Id);

            Console.WriteLine(m1.Title + " - " +m1.Content);

            requester.Update<User>("user", lu[0]);

            //DelUser(lu[0].Id, ut.Token);
            Console.WriteLine();
            Console.WriteLine("Liste après Update");

            lu = requester.Get<List<User>>("user");

            foreach (User item in lu)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Email);
                Console.WriteLine(item.IsAdmin);
            }

            requester.Delete("message", lm[0].Id);
            Console.WriteLine();
            lm = requester.Get<List<Message>>("message");
            foreach (Message item in lm)
            {
                Console.WriteLine("Titre : " + item.Title);
                Console.WriteLine("Contenu : " + item.Content);
            }


            //Message m = new Message { Title = "Titre du message", Content = "lorem ipsum et le reste du blabla" };

            //AddMessage(m, ut.Token);

            //List<Message> liste;
            //liste = GetMessages(ut.Token);

            //Console.WriteLine("liste avant delete");

            //foreach (Message item in liste)
            //{
            //    Console.WriteLine("Titre : "+item.Title);
            //    Console.WriteLine("Contenu : "+item.Content);
            //}

            //liste[0].Content = "Modification du message";

            //UpdateMessage(liste[0], ut.Token);

            //Console.WriteLine();
            //Console.WriteLine("Après Update");
            //Console.WriteLine();

            ////DelMessage(liste[0].Id, ut.Token);

            //liste = GetMessages(ut.Token);
            //foreach (Message item in liste)
            //{
            //    Console.WriteLine("Titre : " + item.Title);
            //    Console.WriteLine("Contenu : " + item.Content);
            //}


            Console.WriteLine("Hello World!");
        }
    }
}
