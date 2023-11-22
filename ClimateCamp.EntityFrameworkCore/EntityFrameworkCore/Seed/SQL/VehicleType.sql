INSERT INTO [Reference].[VehicleTypes] (Id, Name, Description, ModeOfTransport,TransportationKind)
SELECT * FROM ( 
  VALUES
  (newid(),'Bus','', 6, 1),
  (newid(),'Metro', '', 6, 1),
  (newid(),'Passenger Train', '', 2, 1),
  (newid(),'Bicycle', '', 7, 1),
  (newid(),'Electric Bike', '', 7, 1),
  (newid(),'Mixed Public Transportation', '', 6, 1),
  (newid(),'Long haul ('+ NCHAR(0x2265)+ ' 2300 miles)', '', 1, 1),
  (newid(),'Medium haul (' + NCHAR(0x2265) + '300 miles, '+ NCHAR(0x3C) + '2300 miles)', '', 1, 1),
  (newid(),'Short haul ('+ NCHAR(0x3C) + '300 miles)', '', 1, 1),
  (newid(),'Motorcycle', '', 9, 1)
) AS s(Id, Name, Description, ModeOfTransport,TransportationKind) 
WHERE NOT EXISTS ( 
SELECT * FROM [Reference].[VehicleTypes] t WITH (updlock) 
WHERE s.[Name] = t.[Name]);