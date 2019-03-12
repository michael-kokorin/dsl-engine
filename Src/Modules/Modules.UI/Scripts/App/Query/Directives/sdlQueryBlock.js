var Query;
(function (Query) {
    var Directives;
    (function (Directives) {
        function sdlQueryBlock($compile) {
            return {
                scope: {
                    block: "=block",
                    controller: "=controller"
                },
                link: function (scope, element, attrs) {
                    scope.getUrl = function () {
                        var path;
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
                    };
                    var linkFn = $compile("<div ng-include=\"getUrl()\"></div>");
                    var content = linkFn(scope);
                    element.append(content);
                    console.log("Render query block. Type='" + scope.block.$type + "'");
                }
            };
        }
        Directives.sdlQueryBlock = sdlQueryBlock;
        angular.module("Scripts.App.Query").directive("sdlQueryBlock", sdlQueryBlock);
    })(Directives = Query.Directives || (Query.Directives = {}));
})(Query || (Query = {}));
//# sourceMappingURL=sdlQueryBlock.js.map