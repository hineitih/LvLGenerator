# LvLGenerator

## Description

Nous avions pour projet de générer un niveau de platformer en utilisant la génération procédurale. Pour cela, nous avons exploré plusieurs approches :

## Approche constructive
Approche basée sur la recherche avec une fonction de fitness
Fonction de collapse d'onde
Approche constructive

C'est une méthode qui construit le niveau au fur et à mesure, pièce par pièce. Elle est très facile et rapide à implémenter.

Cependant, dès que le niveau est généré, il n'est plus possible de le modifier. Il n'y a pas de système d'auto-évaluation, donc il n'est pas possible de faire des changements dans les cas particuliers où la génération ne nous convient pas.

## Fonction de fitness

C'est une méthode qui construit un niveau aléatoire selon des colonnes disponibles. Après cela, on lui donne la possibilité de s'évaluer. Si son score est inférieur à ce que nous nous sommes fixés, alors on cherche parmi tous ses voisins (c'est-à-dire des clones de ce niveau avec une colonne différente), et dès qu'on a un clone dont le score est supérieur au score de notre niveau actuel, on le remplace.

C'est à nous de définir des règles pour faire évoluer le score. Par exemple, en termes de règles que nous avons fixées :

Proportion des colonnes
Sanction si des colonnes sont isolées, car nous voulons créer des motifs (patterns)
Récompense si certains motifs sont présents
Il est important que les règles ne se contredisent pas et surtout que le niveau que l'on souhaite générer soit réalisable. Si notre score est constamment négatif ou inférieur à notre seuil, alors il nous faut revoir nos règles.

Nous avons utilisé un algorithme de hill climbing, c'est-à-dire que nous prenons toujours le voisin avec un score supérieur. Une des limites de cet algorithme est que l'on risque de se retrouver dans un maximum local et soit ne jamais atteindre le seuil fixé, soit ne pas atteindre le maximum global.

Simulated Annealing (SA) est une technique d'optimisation qui peut surmonter ce problème. Voici une comparaison et une suggestion sur l'opportunité de changer l'algorithme de hill climbing en simulated annealing pour votre projet.
Simulated Annealing est inspiré du processus de recuit en métallurgie. Il commence par une température élevée et la réduit progressivement. À haute température, l'algorithme accepte non seulement les mouvements vers des états de meilleure qualité, mais aussi des mouvements vers des états de qualité inférieure, ce qui permet d'explorer l'espace des solutions plus librement et d'éviter les maxima locaux. Au fur et à mesure que la température diminue, l'algorithme devient plus strict et se comporte davantage comme un hill climbing.

## Fonction de collapse d'onde

La fonction de collapse d'onde (Wave Function Collapse) est une méthode qui génère des niveaux en respectant des contraintes locales tout en minimisant les conflits globaux. Elle fonctionne en réduisant progressivement les possibilités de chaque cellule de la grille du niveau jusqu'à ce qu'une solution cohérente émerge.

Étapes de la méthode :
Initialisation : Toutes les cellules de la grille ont toutes les possibilités.
Sélection : Choisissez une cellule avec le moins de possibilités (ou une au hasard parmi celles-ci).
Propagation : Réduisez les possibilités des cellules voisines en fonction de la cellule sélectionnée.
Répétition : Répétez les étapes de sélection et de propagation jusqu'à ce que toutes les cellules soient déterminées ou qu'une contradiction apparaisse.
Cette méthode est plus complexe à implémenter que les autres, mais elle permet de générer des niveaux plus cohérents et variés, respectant mieux les contraintes de conception.

## Conclusion

Ces différentes approches de génération procédurale de niveaux ont chacune leurs avantages et leurs inconvénients. L'approche constructive est simple mais rigide, la méthode basée sur une fonction de fitness est adaptable mais peut se heurter à des maxima locaux, et la fonction de collapse d'onde offre une grande flexibilité mais au prix d'une complexité accrue. Le choix de la méthode dépend des contraintes spécifiques et des objectifs du projet de développement de jeu.
