module Query.Directives {

    export function sdlQueryBlock($compile: ng.ICompileService): ng.IDirective {
        return {
            scope: {
                block: "=block",
                controller: "=controller"
            },
            link(scope: any, element: ng.IAugmentedJQuery, attrs: ng.IAttributes)
            {
                scope.getUrl = () =>
                {
                    var path: string;

                    switch (scope.block.$type) {
                        case "Infrastructure.Engines.Dsl.Query.Filter.DslFilterBlock, Infrastructure.Engines.Dsl":
                            path = "../Scripts/App/Query/Directives/BlockRenderer/FilterQueryBlock.html";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslFormatBlock, Infrastructure.Engines.Dsl":
                            path = "../Scripts/App/Query/Directives/BlockRenderer/FormatQueryBlock.html";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslGroupBlock, Infrastructure.Engines.Dsl":
                            path = "../Scripts/App/Query/Directives/BlockRenderer/GroupQueryBlock.html";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslOrderBlock, Infrastructure.Engines.Dsl":
                            path = "../Scripts/App/Query/Directives/BlockRenderer/OrderQueryBlock.html";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslLimitBlock, Infrastructure.Engines.Dsl":
                            path = "../Scripts/App/Query/Directives/BlockRenderer/LimitQueryBlock.html";
                            break;
                        default:
                            path = "";
                    }

                    return path;
                }

                var linkFn = $compile("<div ng-include=\"getUrl()\"></div>");

                var content = linkFn(scope);

                element.append(content);

                console.log("Render query block. Type='" + scope.block.$type + "'");
            }
        }
    }

    angular.module("Scripts.App.Query").directive("sdlQueryBlock", sdlQueryBlock);
}