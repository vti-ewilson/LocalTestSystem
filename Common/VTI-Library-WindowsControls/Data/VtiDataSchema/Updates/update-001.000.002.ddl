/*================================================================================*/
/* Create the VTI users                                                           */
/*================================================================================*/

INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('BOB','vtibob','GROUP10',1)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('DJM','drdan','GROUP10',1)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('GEORGE','gms','GROUP10',1)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('MDB','maddog','GROUP10',1)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('TODD','tas','GROUP10',1)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('ROB','wlr','GROUP10',1)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('OPERATOR','operator','GROUP01',0)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('MAINT','maint','GROUP09',0)
INSERT INTO [Users] ([OpID],[Password],[GroupID],[IsLocked])VALUES('Administrator','admin','GROUP09',0)

GO
