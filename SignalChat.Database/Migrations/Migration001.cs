using ServiceStack.OrmLite;
using SignalChat.Database.Entities;

namespace SignalChat.Database.Migrations;

public class Migration001 : MigrationBase
{
    public override void Up()
    {
        Db.CreateTableIfNotExists<UserEntity>();
        Db.CreateTableIfNotExists<MessageEntity>();
    }

    public override void Down()
    {
        Db.DropTable<MessageEntity>();
        Db.DropTable<UserEntity>();
    }
}
