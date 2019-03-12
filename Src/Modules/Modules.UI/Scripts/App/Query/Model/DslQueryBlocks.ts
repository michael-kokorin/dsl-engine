module Query.Models {

    export interface IDslQueryBlocks {
        $type: string;

        $values: Array<any>;

        $sayHello () : void;
    }

    export class DslQueryBlocks implements IDslQueryBlocks {

        $type: string;

        $values: Array<any>;

        $sayHello = () => void {};

        constructor()
        {
            this.$type = "Infrastructure.Engines.Dsl.Query.IDslQueryBlock[], Infrastructure.Engines.Dsl";

            this.$values = new Array<any>();

            this.$sayHello = () => {
                console.log("Hello");

                return;
            }

            this.$sayHello();

            console.log("Query blocks model created");
        }


    }
}