��C R E A T E   D A T A B A S E   [ b e e r _ r e c o m m e n d a t i o n ]  
 G O  
 U S E   [ b e e r _ r e c o m m e n d a t i o n ]  
 G O  
 / * * * * * *   O b j e c t :     T a b l e   [ d b o ] . [ b e e r _ s t y l e s ]         S c r i p t   D a t e :   1 2 / 2 0 / 2 0 1 6   1 0 : 4 7 : 0 0   A M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ b e e r _ s t y l e s ] (  
 	 [ i d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ b e e r _ i d ]   [ i n t ]   N U L L ,  
 	 [ s t y l e ]   [ v a r c h a r ] ( 2 5 )   N U L L ,  
 P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ i d ]   A S C  
 ) W I T H   ( P A D _ I N D E X   =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E   =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S   =   O N ,   A L L O W _ P A G E _ L O C K S   =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
  
 G O  
 / * * * * * *   O b j e c t :     T a b l e   [ d b o ] . [ b e e r s ]         S c r i p t   D a t e :   1 2 / 2 0 / 2 0 1 6   1 0 : 4 7 : 0 0   A M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ b e e r s ] (  
 	 [ i d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ n a m e ]   [ v a r c h a r ] ( 2 5 5 )   N U L L ,  
 	 [ a b v ]   [ f l o a t ]   N U L L ,  
 	 [ i b u ]   [ f l o a t ]   N U L L ,  
 P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ i d ]   A S C  
 ) W I T H   ( P A D _ I N D E X   =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E   =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S   =   O N ,   A L L O W _ P A G E _ L O C K S   =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
  
 G O  
 / * * * * * *   O b j e c t :     T a b l e   [ d b o ] . [ b e e r s _ b r e w e r i e s ]         S c r i p t   D a t e :   1 2 / 2 0 / 2 0 1 6   1 0 : 4 7 : 0 0   A M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ b e e r s _ b r e w e r i e s ] (  
 	 [ i d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ b e e r _ i d ]   [ i n t ]   N U L L ,  
 	 [ b r e w e r y _ i d ]   [ i n t ]   N U L L ,  
 P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ i d ]   A S C  
 ) W I T H   ( P A D _ I N D E X   =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E   =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S   =   O N ,   A L L O W _ P A G E _ L O C K S   =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
  
 G O  
 / * * * * * *   O b j e c t :     T a b l e   [ d b o ] . [ b r e w e r i e s ]         S c r i p t   D a t e :   1 2 / 2 0 / 2 0 1 6   1 0 : 4 7 : 0 0   A M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ b r e w e r i e s ] (  
 	 [ i d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ n a m e ]   [ v a r c h a r ] ( 2 5 5 )   N U L L ,  
 	 [ l o c a t i o n ]   [ v a r c h a r ] ( 2 5 5 )   N U L L ,  
 P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ i d ]   A S C  
 ) W I T H   ( P A D _ I N D E X   =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E   =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S   =   O N ,   A L L O W _ P A G E _ L O C K S   =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
  
 G O  
 / * * * * * *   O b j e c t :     T a b l e   [ d b o ] . [ f a v o r i t e s ]         S c r i p t   D a t e :   1 2 / 2 0 / 2 0 1 6   1 0 : 4 7 : 0 0   A M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ f a v o r i t e s ] (  
 	 [ i d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ b e e r _ i d ]   [ i n t ]   N U L L ,  
 	 [ u s e r _ i d ]   [ i n t ]   N U L L ,  
 	 [ r a t i n g ]   [ i n t ]   N U L L ,  
 P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ i d ]   A S C  
 ) W I T H   ( P A D _ I N D E X   =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E   =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S   =   O N ,   A L L O W _ P A G E _ L O C K S   =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
  
 G O  
 / * * * * * *   O b j e c t :     T a b l e   [ d b o ] . [ u s e r s ]         S c r i p t   D a t e :   1 2 / 2 0 / 2 0 1 6   1 0 : 4 7 : 0 0   A M   * * * * * * /  
 S E T   A N S I _ N U L L S   O N  
 G O  
 S E T   Q U O T E D _ I D E N T I F I E R   O N  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ u s e r s ] (  
 	 [ i d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ n a m e ]   [ v a r c h a r ] ( 2 5 )   N U L L ,  
 	 [ p a s s w o r d ]   [ v a r c h a r ] ( 2 5 5 )   N U L L ,  
 P R I M A R Y   K E Y   C L U S T E R E D    
 (  
 	 [ i d ]   A S C  
 ) W I T H   ( P A D _ I N D E X   =   O F F ,   S T A T I S T I C S _ N O R E C O M P U T E   =   O F F ,   I G N O R E _ D U P _ K E Y   =   O F F ,   A L L O W _ R O W _ L O C K S   =   O N ,   A L L O W _ P A G E _ L O C K S   =   O N )   O N   [ P R I M A R Y ]  
 )   O N   [ P R I M A R Y ]  
  
 G O  
 
