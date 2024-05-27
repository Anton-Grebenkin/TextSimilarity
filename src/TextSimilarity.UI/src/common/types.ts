export enum LogicalOperatorType {
    And,
    Or
}

export enum ExpressionOperatorType {
    Equal,
    Contains,
    In,
    GreaterThan,
    GreaterThanOrEqual,
    IsNull,
    LessThan,
    LessThanOrEqual,
    NotEqual
}

export interface FilterNode {
    logicalOperator?: LogicalOperatorType;
    propertyName?: string;
    expressionOperator?: ExpressionOperatorType;
    value?: any; 
    filterNodes?: FilterNode[]; 
}


export interface Filter {
    take?: number | null;
    skip?: number | null;
    mainNode?: FilterNode;
    sorts: PropertySort[];
}

export interface PropertySort {
    propertyName: string;
    desc: boolean;
}