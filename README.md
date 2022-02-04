# primel-helper
Simple console application to help solve primel puzzles.

# Usage
when running the application takes up to 3 arguments. 
1. The positional known / unknowns. For example if you know that 6 is in 3rd spot, you might put `**6**`.
2. The known possible values. Here you want to include any value that hasn't definitively been ruled out.
3. The known unused values. If we know that 789 are not used, we can additionally add those to the end of our search. `**6** 1234560 789`

# Example

```
search (**6** validvalues invalidvalues):
**6** 163 2457890
=======================
11633
13613
13633
16631
16633
16661
31663
33613
61613
61631
63611
```

![image](https://user-images.githubusercontent.com/976977/152486274-aa5be1ef-6efb-4d8f-b077-48eb64371239.png)
