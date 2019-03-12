module Table {
    export class TableColumn {
        id: string;
        name: string;
        order: number;
    }

    export class TableRowItem {
        columnId: string;
        value: string;
    }

    export class TableRow {
        items: Array<TableRowItem>;
    }

    export class Table {

        columns: Array<TableColumn>;

        rows: Array<TableRow>;
    }
}