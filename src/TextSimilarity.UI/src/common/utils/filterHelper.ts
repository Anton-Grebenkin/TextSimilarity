import { MRT_ColumnFilterFnsState, MRT_ColumnFiltersState, MRT_SortingState } from "material-react-table";
import { ExpressionOperatorType, FilterNode, LogicalOperatorType, PropertySort } from "../types";

export function MapSorts(sorts: MRT_SortingState) : PropertySort[] {
    var result: PropertySort[] = [];
    sorts.forEach(s =>{
        result.push({desc: s.desc, propertyName: s.id})
    })
    return result;
}

export function MapFilters(filters: MRT_ColumnFiltersState, filterTypes: MRT_ColumnFilterFnsState) : FilterNode {
    var subFilters: FilterNode[] = [];
    filters.forEach(s =>{
        subFilters.push({propertyName: s.id, value: s.value, expressionOperator: MapOperatorType(filterTypes[s.id])})
    })
    var result: FilterNode = {
        filterNodes: subFilters,
        logicalOperator: LogicalOperatorType.And
    }
    return result;
}
//['lessThan', 'greaterThan', 'equals', 'notEquals']
function MapOperatorType(filterOperator: string): ExpressionOperatorType{
    switch (filterOperator) {
        case 'equals':
            return ExpressionOperatorType.Equal
        case 'lessThan':
            return ExpressionOperatorType.LessThan
        case 'greaterThan':
            return ExpressionOperatorType.GreaterThan
        case 'notEquals':
            return ExpressionOperatorType.NotEqual
        case 'contains':
            return ExpressionOperatorType.Contains
        default:
            return ExpressionOperatorType.Equal
    }
}