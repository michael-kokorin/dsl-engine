var Query;
(function (Query) {
    var Directives;
    (function (Directives) {
        function sdlFilterBlockSpecification($compile) {
            return {
                scope: {
                    block: "=block",
                    specification: "=specification",
                    parent: "=parent",
                    parentParent: "=parentparent",
                    controller: "=controller"
                },
                link: function (scope, element, attrs) {
                    // ReSharper disable once CoercedEqualsUsingWithNullUndefined
                    if (scope.block.Specification == null) {
                        scope.block.Specification = {
                            $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterSpecification, Infrastructure.Engines.Dsl",
                            LeftSpecification: {
                                $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConstantSpecification, Infrastructure.Engines.Dsl",
                                Value: ""
                            },
                            Operator: 2,
                            RightSpecification: {
                                $type: "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConstantSpecification, Infrastructure.Engines.Dsl",
                                Value: ""
                            }
                        };
                    }
                    scope.getUrl = function () {
                        if (scope.specification == null)
                            return null;
                        if (scope.specification.$type == null)
                            return null;
                        var path;
                        switch (scope.specification.$type) {
                            case "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConditionSpecification, Infrastructure.Engines.Dsl":
                                path = "../Scripts/App/Query/Directives/BlockRenderer/FilterBlockSpecifications/FilterCondition.html";
                                break;
                            case "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterGroupSpecification, Infrastructure.Engines.Dsl":
                                path = "../Scripts/App/Query/Directives/BlockRenderer/FilterBlockSpecifications/FilterGroup.html";
                                break;
                            case "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterConstantSpecification, Infrastructure.Engines.Dsl":
                            case "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterParameterSpecification, Infrastructure.Engines.Dsl":
                                path = "../Scripts/App/Query/Directives/BlockRenderer/FilterBlockSpecifications/FilterConstant.html";
                                break;
                            case "Infrastructure.Engines.Dsl.Query.Filter.Specification.FilterSpecification, Infrastructure.Engines.Dsl":
                                path = "../Scripts/App/Query/Directives/BlockRenderer/FilterBlockSpecifications/FilterSpecification.html";
                                break;
                            default:
                                path = "";
                        }
                        return path;
                    };
                    var linkFn = $compile("<div ng-include=\"getUrl()\"></div>");
                    var content = linkFn(scope);
                    element.append(content);
                }
            };
        }
        Directives.sdlFilterBlockSpecification = sdlFilterBlockSpecification;
        angular.module("Scripts.App.Query").directive("sdlFilterBlockSpecification", sdlFilterBlockSpecification);
    })(Directives = Query.Directives || (Query.Directives = {}));
})(Query || (Query = {}));
//# sourceMappingURL=sdlFilterBlockSpecification.js.map