var Query;
(function (Query) {
    var Directives;
    (function (Directives) {
        function sdlQueryBlockName($compile) {
            return {
                scope: {
                    block: "=block"
                },
                link: function (scope, element, attrs) {
                    var blockTitle;
                    switch (scope.block.$type) {
                        case "Infrastructure.Engines.Dsl.Query.Filter.DslFilterBlock, Infrastructure.Engines.Dsl":
                            blockTitle = "<span class=\"glyphicon glyphicon-filter\"></span>&nbsp;Filter";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslFormatBlock, Infrastructure.Engines.Dsl":
                            blockTitle = "<span class=\"glyphicon glyphicon-menu-hamburger\"></span>&nbsp;Format";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslGroupBlock, Infrastructure.Engines.Dsl":
                            blockTitle = "<span class=\"glyphicon glyphicon-compressed\"></span>&nbsp;Group";
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslOrderBlock, Infrastructure.Engines.Dsl":
                            blockTitle = "<span class=\"glyphicon glyphicon-random\"></span>&nbsp;Order";
                            ;
                            break;
                        case "Infrastructure.Engines.Dsl.Query.DslLimitBlock, Infrastructure.Engines.Dsl":
                            blockTitle = "<span class=\"glyphicon glyphicon-scissors\"></span>&nbsp;Limit";
                            ;
                            break;
                        default:
                            blockTitle = scope.block.$type;
                            break;
                    }
                    blockTitle = "<label>" + blockTitle + "</label>";
                    var linkFn = $compile(blockTitle);
                    var content = linkFn(scope);
                    element.append(content);
                    console.log("Render query block title. Type='" + scope.block.$type + "', Title='" + blockTitle + "'");
                }
            };
        }
        Directives.sdlQueryBlockName = sdlQueryBlockName;
        angular.module("Scripts.App.Query").directive("sdlQueryBlockName", sdlQueryBlockName);
    })(Directives = Query.Directives || (Query.Directives = {}));
})(Query || (Query = {}));
//# sourceMappingURL=sdlQueryBlockName.js.map