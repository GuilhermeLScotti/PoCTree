using System;
using System.Runtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using System.Windows.Input;
using PoCTree.Commands;

namespace PoCTree.Services
{
    public class MainWindowViewModel
    {
        public SourceCache<IDto, string> Source = new SourceCache<IDto, string>(x => x.GetMyId());

        private ReadOnlyObservableCollection<MyNode> _mainItems;
        public ReadOnlyObservableCollection<MyNode> MainItems => _mainItems;

        public Command AddSomethingCommand { get; }
        public Command MoveCommand { get; }

        public MainWindowViewModel()
        {
            List<IDto> dtos = new List<IDto>();

            AddressDto address1 = new AddressDto(1);
                QuotationDto quotation1 = new QuotationDto(1, 1);
                QuotationDto quotation2 = new QuotationDto(1, 2);
                ProjectDto project1 = new ProjectDto(1, 1, "1st");

            AddressDto address2 = new AddressDto(2);
                QuotationDto quotation3 = new QuotationDto(2, 3);
                QuotationDto quotation4 = new QuotationDto(2, 4);
                ProjectDto project2 = new ProjectDto(2, 2, "2nd");

            FolderDto quotFolder1 = new FolderDto(1, DtoType.Quotation);
            FolderDto projFolder1 = new FolderDto(1, DtoType.Project);
            FolderDto quotFolder2 = new FolderDto(2, DtoType.Quotation);
            FolderDto projFolder2 = new FolderDto(2, DtoType.Project);

            #region add to list
            dtos.Add(address1);
            dtos.Add(quotation1);
            dtos.Add(quotation2);
            dtos.Add(project1);
            dtos.Add(address2);
            dtos.Add(quotation3);
            dtos.Add(quotation4);
            dtos.Add(project2);
            dtos.Add(quotFolder1);
            dtos.Add(quotFolder2);
            dtos.Add(projFolder1);
            dtos.Add(projFolder2);
            #endregion

            Source.AddOrUpdate(dtos);

            bool DefaultPredicate(Node<IDto, string> node) => node.IsRoot;
            
            var tree = Source.Connect()
                .TransformToTree(dto => dto.GetMyParentId(), Observable.Return((Func<Node<IDto, string>, bool>)DefaultPredicate))
                .Transform(n => new MyNode(n))
                .Sort(SortExpressionComparer<MyNode>.Ascending(node =>
                {
                    switch (node.Item.DtoType)
                    {
                        case DtoType.Address:
                            return Convert.ToInt32((((int)node.Item.DtoType).ToString() + node.Item.AddressId.ToString()));

                        case DtoType.Project:
                            return Convert.ToInt32((((int)node.Item.DtoType).ToString() + node.Item.AddressId.ToString() + (node.Item as ProjectDto).ProjectId.ToString()));

                        case DtoType.Quotation:
                            return Convert.ToInt32((((int)node.Item.DtoType).ToString() + node.Item.AddressId.ToString() + (node.Item as QuotationDto).QuotationId.ToString()));

                        default:
                            return node.Item.AddressId;
                    }
                }))
                .Bind(out _mainItems)
                .DisposeMany()
                .Subscribe();            

            AddSomethingCommand = new Command(AddSomethingNew);
            MoveCommand = new Command(MoveProjectQuotation);
        }

        private void AddSomethingNew()
        {
            Source.AddOrUpdate(new ProjectDto(1, 3, "3rd"));
            Source.AddOrUpdate(new ProjectDto(2, 4, "4th"));
            Source.AddOrUpdate(new ProjectDto(1, 5, "5th"));
            Source.AddOrUpdate(new ProjectDto(2, 6, "6th"));
            Source.AddOrUpdate(new ProjectDto(3, 7, "7th"));
            Source.AddOrUpdate(new ProjectDto(4, 8, "8th"));
            Source.AddOrUpdate(new ProjectDto(5, 9, "9th"));
            Source.AddOrUpdate(new ProjectDto(1, 1, "New Name!"));

            Source.AddOrUpdate(new QuotationDto(2, 5));
            Source.AddOrUpdate(new QuotationDto(1, 6));
            Source.AddOrUpdate(new QuotationDto(2, 7));

            var toRemove = MainItems.Where(i => !(i.Item is AddressDto)).ToList();

            toRemove.ForEach(i => Source.Remove(i.Item));
        }

        private void MoveProjectQuotation()
        {
            var project = Source.Items.Where(x => x.DtoType == DtoType.Project && x.AddressId == 1 && (x as ProjectDto).ProjectId == 1).FirstOrDefault();

            Source.Remove(project);

            project.AddressId = 2;

            Source.AddOrUpdate(project);
        }
    }

    public class MyNode
    {
        private ReadOnlyObservableCollection<MyNode> _subItems;
        public ReadOnlyObservableCollection<MyNode> SubItems => _subItems;
        public IDto Item { get; }
        public MyNode(Node<IDto, string> baseNode)
        {
            Item = baseNode.Item;

            var tree = baseNode.Children.Connect()
                .Transform(n => new MyNode(n))
                .Sort(SortExpressionComparer<MyNode>.Ascending(node =>
                {
                    switch (node.Item.DtoType)
                    {
                        case DtoType.Address:
                            return Convert.ToInt32((((int)node.Item.DtoType).ToString() + node.Item.AddressId.ToString()));

                        case DtoType.Project:
                            return Convert.ToInt32((((int)node.Item.DtoType).ToString() + node.Item.AddressId.ToString() + (node.Item as ProjectDto).ProjectId.ToString()));

                        case DtoType.Quotation:
                            return Convert.ToInt32((((int)node.Item.DtoType).ToString() + node.Item.AddressId.ToString() + (node.Item as QuotationDto).QuotationId.ToString()));

                        default:
                            return node.Item.AddressId;
                    }
                }))
                .Bind(out _subItems)
                .DisposeMany()
                .Subscribe();
        }
    }
}
