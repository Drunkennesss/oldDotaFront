OPTIONS (SKIP=1)
LOAD DATA
INFILE '/Dota_heroes_final_all_range.csv'
APPEND
INTO TABLE alex.heroes
FIELDS TERMINATED BY ";" trailing nullcols
(
	id,  
	name,
	main_stat,
	stren,
	agil,
	intel,
	stren_incr float external,
	agil_incr float external,	
	int_incr float external,
	hp_base,
	mana_base,	
	hp_regen_base float external,	
	mana_regen_base float external,
	damage_base,
	armour_base,
	spellresist_base float external,	
	movespeed_base,
	attack_range,
	attack_speed_base float external,	
	attack_type,
	projectile_speed,	
	view_range
	
)
	                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  