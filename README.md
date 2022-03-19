# SimpleJWTAuth

Implémentation d'autorisation par json web token 

Contient : 

Une api fournissant : 
* Une route permettant l'enregistrement d'un compte utilisateur
* Une route pour l'obtention d'un token + refresh token
* Une route permettant le rafraichissement du jeton

Une api crud de test nécessitant une autorisation et paramétrée avec l'api précédente comme emetteur de jwt

Les deux apis fournissent un swagger

![demo](jwt.gif)

## Données

Les données sont gérées avec entityframework qui utilise un connecteur pour mysql

Modifier la chaine de connexion et le connecteur en fonction du besoin
