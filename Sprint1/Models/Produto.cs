using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint1.Models
{
    [Table("PRODUTOS")]
    public class Produto
    {
        [Key]
        [Column("ID")]
        public long Produto_ID { get; set; }


        [Required]
        [Column("NOME")]
        [Display(Name = "Nome")]
        public string Titulo { get; set; }

        [Required]
        [Column("DESCRICAO")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [Column("ENDERECO_IMAGEM")]
        [Display(Name = "URL da Imagem")]
        public string Imagem_url { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Column("PRECO")]
        [Display(Name = "Preço")]
        public double Preco { get; set; }

        [Required]
        [Column("PRECO_ORIGINAL")]
        public double Preco_original { get; set; }

        [Column("PESO")]
        public double? Peso { get; set; }

        [Required]
        [Column("ESTOQUE")]
        public int Estoque { get; set; }

        [Required]
        [Column("CONDICAO")]
        [Display(Name = "Condição")]
        public int Condicao_produto { get; set; }

        [Required]
        [Column("ALTURA")]
        public float Altura { get; set; }

        [Required]
        [Column("LARGURA")]
        public float Largura { get; set; }

        [Required]
        [Column("PROFUNDIDADE")]
        public float Profundidade { get; set; }

        [Required]
        [Column("FUNCIONARIO_ID")]
        public long FuncionarioId { get; set; }

        [Required(ErrorMessage = "O Autor é obrigatório.")]
        [Column("AUTOR")]
        [StringLength(100, ErrorMessage = "O nome do autor não pode exceder 100 caracteres.")]
        [Display(Name = "Autor")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Column("CATEGORIA")]
        [Display(Name = "Categoria")]
        public int Categoria { get; set; }
    }
}