import { IType } from './productType';


export interface ITypePagination {
    pageIndex: number
    pageSize: number
    count: number
    data: IType[]
}
