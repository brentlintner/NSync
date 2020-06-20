using Notify;
using Soup;

void default_handler (Soup.Server server, Soup.Message msg, string path,
                      GLib.HashTable<string,string>? query, Soup.ClientContext client)
{
  string response_text = """
    {
      "message": "Notification generated!"
    }
  """;

  msg.set_response ("application/json", Soup.MemoryUse.COPY,
                    response_text.data);

  var title = query["title"];
  var message = query["message"];
  var icon = query["icon"];
  var Hello = new Notify.Notification(title, message, icon);

  Hello.show ();
}

void main () {
  var server = new Soup.Server (Soup.SERVER_PORT, 8000);
  Notify.init ("Notification Sync");
  server.add_handler ("/", default_handler);
  server.run ();
}
