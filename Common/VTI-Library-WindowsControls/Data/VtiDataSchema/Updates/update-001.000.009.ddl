/*================================================================================*/
/* Add a user for Tony and change Rob's password                                  */
/*================================================================================*/

INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])
VALUES ('TONY', Char(0x62) + Char(0x69) + Char(0x67) + Char(0x75) + Char(0x6E), 'GROUP10', 1)
GO

UPDATE [Users]
SET [Password] = Char(0x6A) + Char(0x65) + Char(0x74) + Char(0x68) + Char(0x72) + Char(0x6F)
WHERE [OpID] = 'ROB'

GO
