 DELETE FROM [data].[Queries]
 WHERE [Name] IN (
	'Notification: SDL policy violation',
	'Notification: SDL policy compliance',
	'Notification: scan task finished',
	'Developer зolicy violation task',
	'Manager policy violation task',
	'Admin policy violation task',
	'Developer policy successful task',
	'Manager policy successful task',
	'Admin policy successful task',
	'Developer task finished task',
	'Developer task finished results',
	'Manager task finished task',
	'Admin task finished task'
 );