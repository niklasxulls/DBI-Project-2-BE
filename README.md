# Stackblob
## Entity Configuration Conventions

<br>

**Cascades**


- Whenever a table references an other table multiple times, the delete behavior 
should be set to strict, since SQL Server can't cope with multiple cascade paths

- Cascades are only defined in the file that's affected by the cascade