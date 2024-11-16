import { IUser } from './user';


export interface IUserPagination {
    pageIndex: number
    pageSize: number
    count: number
    data: IUser[]
}
