﻿SELECT IIF(COUNT(1) > 0, 1, 0) FROM Track t, Musician m, Musician_Track mt  where t.IdTrack = mt.IdTrack AND m.IdMusician = mt.IdMusician and t.IdAlbum IS NOT NULL AND m.IdMusician = 1