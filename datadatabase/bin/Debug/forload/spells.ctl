OPTIONS (SKIP=1)
LOAD DATA
INFILE '/spellsonly_final.csv'
APPEND
INTO TABLE alex.spells
FIELDS TERMINATED BY ";" trailing nullcols
(                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                	ID,
	DESCRIPTION char(4000),
	SPELL_TYPE,
	COOLDOWN,
	MANACOST
)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  