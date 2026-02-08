import { Component, TemplateRef, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  TodoListsClient, TodoItemsClient, TodoTagsClient,
  TodoListDto, TodoItemDto, PriorityLevelDto,
  CreateTodoListCommand, UpdateTodoListCommand,
  CreateTodoItemCommand, UpdateTodoItemDetailCommand, ColourDto, TagDto, TodoItemTagDto, CreateTagToTodoItemCommand
} from '../web-api-client';

@Component({
  selector: 'app-todo-component',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.scss']
})
export class TodoComponent implements OnInit {
  debug = false;
  deleting = false;
  isAddingTag = false;
  deleteCountDown = 0;
  deleteCountDownInterval: any;
  lists: TodoListDto[];
  priorityLevels: PriorityLevelDto[];
  supportedColours: ColourDto[];
  selectedList: TodoListDto;
  selectedItem: TodoItemDto;
  filteredTodoItems: TodoItemDto[] = [];
  topTags: string[] = [];
  tagsList: TagDto[] = [];
  newListEditor: any = {};
  listOptionsEditor: any = {};
  tags: TodoItemTagDto[] = [];
  newListModalRef: BsModalRef;
  listOptionsModalRef: BsModalRef;
  deleteListModalRef: BsModalRef;
  itemDetailsModalRef: BsModalRef;
  searchTimeout: any;
  filterTag: any;
  itemDetailsFormGroup = this.fb.group({
    id: [null],
    listId: [null],
    priority: [''],
    colour: [''],
    newTag: [''],
    note: ['']
  });
  itemSearchFormGroup = this.fb.group({
    searchText: [''],
    filterTag: ['']
  });


  constructor(
    private listsClient: TodoListsClient,
    private itemsClient: TodoItemsClient,
    private tagsClient: TodoTagsClient,
    private modalService: BsModalService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.listsClient.get().subscribe(
      result => {
        this.lists = result.lists;
        this.priorityLevels = result.priorityLevels;
        this.supportedColours = result.supportedColours;

        if (this.lists.length) {
          this.onListSelection(this.lists[0]);
        }
      },
      error => console.error(error)
    );
    this.tagsClient.get().subscribe(result => {
      this.topTags = result.list;
      this.tagsList = [{ name: 'All', id: 0 } as TagDto, ...result.tags]
      this.itemSearchFormGroup.patchValue({filterTag: 0});
      },
      error => console.error(error)
    );
  }
  onListSelection(list: TodoListDto) {
    this.selectedList = list;
    this.filteredTodoItems = this.selectedList.items;
  }
  searchTodoItems() {
    clearTimeout(this.searchTimeout);
    this.searchTimeout = setTimeout(() => {
      var searchText = this.itemSearchFormGroup.get("searchText")?.value;

      this.filterTag = this.itemSearchFormGroup.get("filterTag")?.value;

      this.filteredTodoItems = this.selectedList.items.filter(item => {
        const matchesTag = this.filterTag === 0 || item.tags?.some(t => t.tagId === this.filterTag);
        const matchesSearch = !searchText || item.title.toLowerCase().includes(searchText.toLowerCase());
        return matchesSearch && matchesTag;
      });
    }, 300);
   
  }
  getRandomColour(): string {
    const randomIndex = Math.floor(Math.random() * this.supportedColours.length);
    return this.supportedColours[randomIndex].code;
  }


  reloadData(): void {
    this.listsClient.get().subscribe(
      result => {
        this.lists = result.lists;
        this.priorityLevels = result.priorityLevels;
        this.supportedColours = result.supportedColours;

        if (this.lists.length) {
          this.onListSelection(this.lists[0]);
        }
      },
      error => console.error(error)
    );
  }

  // Lists
  remainingItems(list: TodoListDto): number {
    return list.items.filter(t => !t.done).length;
  }

  showNewListModal(template: TemplateRef<any>): void {
    this.newListModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('title').focus(), 250);
  }

  newListCancelled(): void {
    this.newListModalRef.hide();
    this.newListEditor = {};
  }
  showAddTag(): void {
    this.isAddingTag = true;
  }
  
  addTag(): void {
    const newTag = {
      todoItemId: this.selectedItem.id,
      name: this.itemDetailsFormGroup.get("newTag")?.value
    } as CreateTagToTodoItemCommand;

    this.tagsClient.create(newTag).subscribe(
      result => {
        const tagDto = {
          todoItemId: newTag.todoItemId,
          tagId: result,
          tag: {
            id: result,
            name: newTag.name
          } as TagDto
        } as TodoItemTagDto;
        if (!this.selectedItem.tags && !this.tags) {
          this.selectedItem.tags = []
          this.tags = []
        }
        this.selectedItem.tags.push(tagDto);
        this.tags = this.selectedItem.tags;
      },
      error => {
        const errors = JSON.parse(error.response);
        if (errors && errors.Title) {
          this.newListEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('newTag').focus(), 250);
      }
    );
    this.isAddingTag = false;
  }

  addTagInLine(todoItemId: number, tagName: string): void {
    const newTag = {
      todoItemId: todoItemId,
      name: tagName
    } as CreateTagToTodoItemCommand;

    this.tagsClient.create(newTag).subscribe(
      result => {
        const tagDto = {
          todoItemId: newTag.todoItemId,
          tagId: result,
          tag: {
            id: result,
            name: newTag.name
          } as TagDto
        } as TodoItemTagDto;
        if (!this.selectedItem.tags && !this.tags) {
          this.selectedItem.tags = []
          this.tags = []
        }
        this.selectedItem.tags.push(tagDto);
        this.tags.push(tagDto);
      },
      error => {
        const errors = JSON.parse(error.response);
        if (errors && errors.Title) {
          this.newListEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('newTag').focus(), 250);
      }
    );
    this.isAddingTag = false;
  }

  cancelTag(): void {
    this.isAddingTag = false;
  }
  addList(): void {
    const list = {
      id: 0,
      title: this.newListEditor.title,
      items: []
    } as TodoListDto;

    this.listsClient.create(list as CreateTodoListCommand).subscribe(
      result => {
        list.id = result;
        this.lists.push(list);
        this.onListSelection(this.lists[0]);
        this.newListModalRef.hide();
        this.newListEditor = {};
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newListEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('title').focus(), 250);
      }
    );
  }

  showListOptionsModal(template: TemplateRef<any>) {
    this.listOptionsEditor = {
      id: this.selectedList.id,
      title: this.selectedList.title
    };

    this.listOptionsModalRef = this.modalService.show(template);
  }

  updateListOptions() {
    const list = this.listOptionsEditor as UpdateTodoListCommand;
    this.listsClient.update(this.selectedList.id, list).subscribe(
      () => {
        (this.selectedList.title = this.listOptionsEditor.title),
          this.listOptionsModalRef.hide();
        this.listOptionsEditor = {};
      },
      error => console.error(error)
    );
  }

  confirmDeleteList(template: TemplateRef<any>) {
    this.listOptionsModalRef.hide();
    this.deleteListModalRef = this.modalService.show(template);
  }

  deleteListConfirmed(): void {
    this.listsClient.delete(this.selectedList.id).subscribe(
      () => {
        this.deleteListModalRef.hide();
        this.selectedList.deletedOn = new Date(Date.now());
        this.lists = this.lists.filter(t => t.id !== this.selectedList.id || t.deletedOn === undefined);
        if (this.lists.length) {
          this.onListSelection(this.lists[0]);
        }
        else {
          this.selectedList = null;
        }
      },
      error => console.error(error)
    );
  }
  removeTag(tag: TodoItemTagDto): void
  {
    this.tagsClient.delete(tag.todoItemId, tag.tagId).subscribe(
      () => {
        this.selectedItem.tags = this.selectedItem.tags.filter(t => t.tagId !== tag.tagId);
        this.tags = this.selectedItem.tags;
      },
      error => console.error(error)
    )
  }

  // Items
  showItemDetailsModal(template: TemplateRef<any>, item: TodoItemDto): void {
    this.selectedItem = item;
    this.tags = item.tags;
    this.itemDetailsFormGroup.patchValue(this.selectedItem);
    this.itemDetailsModalRef = this.modalService.show(template);
    this.itemDetailsModalRef.onHidden.subscribe(() => {
        this.stopDeleteCountDown();
    });
  }

  updateItemDetails(): void {
    const item = new UpdateTodoItemDetailCommand(this.itemDetailsFormGroup.value);
    this.itemsClient.updateItemDetails(this.selectedItem.id, item).subscribe(
      () => {
        if (this.selectedItem.listId !== item.listId) {
          this.selectedList.items = this.selectedList.items.filter(
            i => i.id !== this.selectedItem.id
          );
          const listIndex = this.lists.findIndex(
            l => l.id === item.listId
          );
          this.selectedItem.listId = item.listId;
          this.lists[listIndex].items.push(this.selectedItem);
        }

        this.selectedItem.priority = item.priority;
        this.selectedItem.note = item.note;
        this.selectedItem.colour = item.colour;
        this.itemDetailsModalRef.hide();
        this.itemDetailsFormGroup.reset();
      },
      error => console.error(error)
    );
  }

  addItem() {
    const item = {
      id: 0,
      listId: this.selectedList.id,
      priority: this.priorityLevels[0].value,
      title: '',
      colour: this.getRandomColour(),
      done: false,
      tags: [{ tagId: this.filterTag, todoItemId: 0 } as TodoItemTagDto]
    } as TodoItemDto;
    this.selectedList.items.push(item);
    this.searchTodoItems();
    const index = this.selectedList.items.length - 1;
    this.editItem(item, 'itemTitle' + index);
  }

  editItem(item: TodoItemDto, inputId: string): void {
    this.selectedItem = item;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateItem(item: TodoItemDto, pressedEnter: boolean = false): void {
    const isNewItem = item.id === 0;

    if (!item.title.trim()) {
      this.deleteItem(item);
      return;
    }

    if (item.id === 0) {
      this.itemsClient
        .create({
          ...item, listId: this.selectedList.id
        } as CreateTodoItemCommand)
        .subscribe(
          result => {
            item.id = result;
            var filter = this.tagsList.find(c => c.id === this.filterTag)
            if (filter !== undefined && this.filterTag !==0) {
              this.addTagInLine(item.id, filter.name);
            }
          },
          error => console.error(error)
        );
    } else {
      this.itemsClient.update(item.id, item).subscribe(
        () => console.log('Update succeeded.'),
        error => console.error(error)
      );
    }

    this.selectedItem.colour = item.colour;

    this.selectedItem = null;
   
    if (isNewItem && pressedEnter) {
      setTimeout(() => this.addItem(), 250);
    }
  }

  deleteItem(item: TodoItemDto, countDown?: boolean) {
    if (countDown) {
      if (this.deleting) {
        this.stopDeleteCountDown();
        return;
      }
      this.deleteCountDown = 3;
      this.deleting = true;
      this.deleteCountDownInterval = setInterval(() => {
        if (this.deleting && --this.deleteCountDown <= 0) {
          this.deleteItem(item, false);
        }
      }, 1000);
      return;
    }
    this.deleting = false;
    if (this.itemDetailsModalRef) {
      this.itemDetailsModalRef.hide();
    }
    const itemIndex = this.selectedList.items.indexOf(this.selectedItem);
    if (item.id === 0) {
      this.selectedList.items.splice(itemIndex, 1);
      this.searchTodoItems();

    } else {
      item.deletedOn = new Date(Date.now());
      this.itemsClient.delete(item.id).subscribe(
        () => {
          this.selectedList.items = this.selectedList.items.filter(t => t.id !== item.id || t.deletedOn === undefined)
          this.searchTodoItems();
        },
        error => console.error(error)
      );
    }
  }

  stopDeleteCountDown() {
    clearInterval(this.deleteCountDownInterval);
    this.deleteCountDown = 0;
    this.deleting = false;
  }
}
