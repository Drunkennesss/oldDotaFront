   M S B A M L   `   `   `  ���� G  Cdatadatabase, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:datadatabasedatadatabase  %    datadatabase.InsertUpdateHeroes   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ,localclr-namespace:datadatabase       N� Title$  InsertUpdateHeroes��.��� .+��� $��Name: ��$
��Left=�$��	45,67,0,0q�$	��Top=�$��30��$��91��$��Times New Roman)�$��16(��� $��Mainstat��$
��Left=�$��
45,101,0,0q�$	��Top=�$��36��$��91��$��Times New Roman)�$��16(��� $��
AttackType��$
��Left=�$��
45,144,0,0q�$	��Top=�$��35��$��91��$��Times New Roman)�$��16(��� $��AttackRange��$
��Left=�$��
33,180,0,0q�$	��Top=�$��34��$	��102�� J� RenderTransformOrigin$ 0.5,0.5&�$��Times New Roman)�$��16(�z�V� .~��� ��  �� AngleX$ 1.599Q���  �� Angle$ 0.115Q�T�  T� X$ 0.572Q��� $
��Agil��$
��Left=�$��
45,219,0,0q�$	��Top=�$��32��$��91��$��Times New Roman)�$��16(��� $��Intel��$
��Left=�$��
45,256,0,0q�$	��Top=�$��32��$��91��$��Times New Roman)�$��16(��� $��Stren��$
��Left=�$��
45,293,0,0q�$	��Top=�$��31��$��91��$��Times New Roman)�$��16(��� $��AgilIncr��$
��Left=�$��
45,329,0,0q�$	��Top=�$��32��$��91��$��Times New Roman)�$��16(��� $��IntIncr��$
��Left=�$��
45,366,0,0q�$	��Top=�$��31��$��91��$��Times New Roman)�$��16(��� $��	StrenIncr��$
��Left=�$��
45,402,0,0q�$	��Top=�$��31��$��91��$��Times New Roman)�$��16(��� $��Damage��$
��Left=�$��
45,438,0,0q�$	��Top=�$��31��$��91��$��Times New Roman)�$��16(��� $��HpBase��$
��Left=�$��
45,474,0,0q�$	��Top=�$��32��$��91��$��Times New Roman)�$��16(��� $��ManaBase��$
��Left=�$��
45,511,0,0q�$	��Top=�$��30��$��91��$��Times New Roman)�$��16(��� $��Armor��$
��Left=�$��
45,546,0,0q�$	��Top=�$��31��$��91��$��Times New Roman)�$��16(��� $��AttackSpeed��$
��Left=�$��
45,582,0,0q�$	��Top=�$��91��$��31��$��Times New Roman)�$��16(��� $��	Movespeed��$
��Left=�$��
503,49,0,0q�$	��Top=�$��30��$��92��$��Times New Roman)�$��16(��� $��Spellresist��$
��Left=�$��
503,84,0,0q�$	��Top=�$��36��$��92��$��Times New Roman)�$��16(��� $��HpRegen��$
��Left=�$��503,125,0,0q�$	��Top=�$��30��$��92��$��Times New Roman)�$��16(��� $��	ManaRegen��$
��Left=�$��503,155,0,0q�$	��Top=�$ 1.193,0.297&�$��31��$��92��$��Times New Roman)�$��16(��� $��	ViewRange��$
��Left=�$��503,191,0,0q�$	��Top=�$��92��$��30��$��Times New Roman)�$��16(��� $��	ProjSpeed��$
��Left=�$��503,235,0,0q�$	��Top=�$��92��$��32��$��Times New Roman)�$��16(��� -    ��Name$
 Name��$
��Left=�$��30��$��
141,67,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -    ��Name$ 
AttackType��$
��Left=�$��141,146,0,0q�$	��Top=�$	��207��$��30��$��Times New Roman)�$��16(� �� SelectedIndex$ 0��.@��� $��melee��$��16(�$��Times New Roman)��� $��range��$��16(�$��Times New Roman)��� -   $ AttackRange��$
��Left=�$��33��$��141,181,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $
 Agil��$
��Left=�$��32��$��141,219,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ Intel��$
��Left=�$��32��$��141,256,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ Stren��$
��Left=�$��31��$��141,293,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ AgilIncr��$
��Left=�$��32��$��141,329,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ IntIncr��$
��Left=�$��31��$��141,366,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -	   $ StrIncr��$
��Left=�$��31��$��141,402,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -
   $ Damage��$
��Left=�$��31��$��141,438,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ HpBase��$
��Left=�$��32��$��141,474,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ ManaBase��$
��Left=�$��30��$��141,511,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ Armor��$
��Left=�$��31��$��141,546,0,0q�$
 Wrap=�$	��Top=�$	��207��$ 0.495,0.496&�$��Times New Roman)�$��16(��� -   $ AttackSpeed��$
��Left=�$��31��$��141,582,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ MainStat��$
��Left=�$��141,102,0,0q�$	��Top=�$	��207��$��36��$��Times New Roman)�$��16(�$ 0��.@��� $	��agi��$��16(�$��Times New Roman)��� $	��int��$��16(�$��Times New Roman)��� $	��str��$��16(�$��Times New Roman)��� -   $ 	MoveSpeed��$
��Left=�$��30��$��
610,49,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ SpellResist��$
��Left=�$��36��$��
610,84,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ HpRegen��$
��Left=�$��30��$��610,125,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ 	ManaRegen��$
��Left=�$��26��$��610,160,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ 	ViewRange��$
��Left=�$��30��$��610,191,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $ 	ProjSpeed��$
��Left=�$��32��$��610,235,0,0q�$
 Wrap=�$	��Top=�$	��207��$��Times New Roman)�$��16(��� -   $��Confirm��$
��Left=�$��675,361,0,0q�$	��Top=�$	��100��$��31��$��Times New Roman)�$��16(��� -   $
��Back��$
��Left=�$��675,490,0,0q�$	��Top=�$	��100��$��30��$��Times New Roman)�$��16(��� -   $ AddSpell��$
��Left=�$��31��$��607,272,0,0q�$
 Wrap=�$	��Top=�$	��210��$��Times New Roman)�$��16(��� -   	 ��Name$	 _info��$�� ��$
��Left=�$��
141,22,0,0q�$	��Top=�$	��207��$��40��$��Times New Roman)�$��16(��� -   $ 	_addspell��$
��Left=�$��482,272,0,0q�$	��Top=�$	��120��$��31��$��Times New Roman)�$��16(�.@��� $��AddSpellbyId��
 �� 
IsSelected
 . �� -    ��Name$ _combo_delete��$��DeleteSpellbyId��