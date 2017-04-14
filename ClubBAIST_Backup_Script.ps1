$dt = Get-Date -Format yyyyMMddHHmmss
$dbname = 'ClubBaist'
Backup-SqlDatabase -ServerInstance JLAPPY -Database $dbname -BackupFile "C:\Backups\$($dbname)_db_$($dt).bak" 