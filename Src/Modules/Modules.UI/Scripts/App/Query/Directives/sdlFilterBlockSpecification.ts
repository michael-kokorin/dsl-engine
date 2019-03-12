module Query.Directives {

    export function sdlFilterBlockSpecification($compile: ng.ICompileService): ng.IDirective {
        return {
            scope: {
                block: "=block",
                specification: "=specification",
                parent: "=parent",
                parentParent: "=parentparent",
                controller: "=controller"
            },
            link(scope: any, element: ng.IAugmentedJQuery, attrs: ng.IAttributes)
            {
                // ReSharper disable once CoercedEqualsUsingWithNullUndefined
                if (scope.block.Specification == null)
                {
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

                scope.getUrl = () => {

                    if (scope.specification == null)
                        return null;

                    if (scope.specification.$type == null)
                        return null;

                    var path: string;

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
                }

                var linkFn = $compile("<div ng-include=\"getUrl()\"></div>");

                var content = linkFn(scope);

                element.append(content);
            }
        }
    }

    angular.module("Scripts.App.Query").directive("sdlFilterBlockSpecification", sdlFilterBlockSpecification);
}