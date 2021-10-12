select count(*) from EncomendasCompras
select count(*) from Encomendas


select 
count(*)
--enc.NomeArtigo, compra.Material 
from EncomendasCompras compra
	inner join Encomendas enc on compra.Encomendas_IdEncomenda = enc.IdEncomenda
	where enc.DataPedido < '20180101'

delete compra from EncomendasCompras compra where Encomendas_IdEncomenda is null

delete compra from  EncomendasCompras compra
	inner join Encomendas enc on compra.Encomendas_IdEncomenda = enc.IdEncomenda
	where enc.DataPedido < '20180101'

select count(*) from Encomendas
delete  enc from Encomendas enc
	where enc.DataPedido < '20180101'
select count(*) from Encomendas

select top 10 * from Encomendas