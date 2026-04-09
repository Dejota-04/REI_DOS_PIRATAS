using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprint1.ViewModels
{
    public class ProdutoEditViewModel
    {
        [Required]
        public long Produto_ID { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(150)]
        [Display(Name = "Nome")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo URL da Imagem é obrigatório.")]
        [Display(Name = "URL da Imagem")]
        public string Imagem_url { get; set; }

        [Required(ErrorMessage = "O campo Estoque é obrigatório.")]
        public int Estoque { get; set; }

        [Required(ErrorMessage = "O campo Condição é obrigatório.")]
        [Display(Name = "Condição")]
        public int Condicao_produto { get; set; }

        [Required(ErrorMessage = "O campo Altura é obrigatório.")]
        public float Altura { get; set; }

        [Required(ErrorMessage = "O campo Largura é obrigatório.")]
        public float Largura { get; set; }

        [Required(ErrorMessage = "O campo Profundidade é obrigatório.")]
        public float Profundidade { get; set; }

        [Required(ErrorMessage = "O campo ID do Funcionário é obrigatório.")]
        [Display(Name = "ID do Funcionário")]
        public long FuncionarioId { get; set; }

        [Required(ErrorMessage = "O Autor é obrigatório.")]
        [Display(Name = "Autor")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Display(Name = "Categoria")]
        public int Categoria { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Display(Name = "Preço")]
        public double Preco { get; set; }
    }
}